using Charcutarie.Application.Contracts;
using Charcutarie.Core.ExceptionHandling;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Charcutarie.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderApp orderApp;
        private readonly IPricingApp pricingApp;
        private readonly IProductApp productApp;
        private readonly IOrderItemApp orderItemApp;
        private readonly IMeasureUnitApp measureUnitApp;
        private readonly IDataSheetService dataSheetService;
        private readonly ITransactionApp transactionApp;

        public OrderService(IOrderApp orderApp,
            IPricingApp pricingApp,
            IProductApp productApp,
            IOrderItemApp orderItemApp,
            IMeasureUnitApp measureUnitApp,
            IDataSheetService dataSheetService,
            ITransactionApp transactionApp)
        {
            this.orderApp = orderApp;
            this.pricingApp = pricingApp;
            this.productApp = productApp;
            this.orderItemApp = orderItemApp;
            this.measureUnitApp = measureUnitApp;
            this.dataSheetService = dataSheetService;
            this.transactionApp = transactionApp;
        }
        public async Task<long> Add(NewOrder model, int corpClientId)
        {
            var ids = model.OrderItems.Select(i => i.ProductId).ToList();
            var prods = await productApp.GetRange(corpClientId, ids);
            int i = 1;
            var units = await measureUnitApp.GetAll();
            foreach (var item in model.OrderItems)
            {
                item.ItemNumber = i;
                var prod = prods.FirstOrDefault(p => p.ProductId == item.ProductId);
                item.ProductPrice = prod.Price;
                var pType = units.FirstOrDefault(u => u.MeasureUnitId == prod.MeasureUnitId).MeasureUnitTypeId;
                var qType = units.FirstOrDefault(u => u.MeasureUnitId == item.MeasureUnitId).MeasureUnitTypeId;
                await CalculateItem(item, prod.MeasureUnitId, pType, qType, corpClientId);
                i++;
            }
            return await orderApp.Add(model, corpClientId);
        }
        private async Task CalculateItem(NewOrderItem item, MeasureUnitEnum prodMeasureUnit, MeasureUnitTypeEnum pType, MeasureUnitTypeEnum qType, int corpClientId)
        {
            item.OriginalPrice = pricingApp.CalculatePricePerTotalWeight(new PriceRequest
            {
                ProductMeasureUnit = prodMeasureUnit,
                ProductPrice = item.ProductPrice,
                Quantity = item.Quantity,
                QuantityMeasureUnit = item.MeasureUnitId
            }, pType, qType);
            var prodSummary = await dataSheetService.CalculateProduction(item.ProductId, item.MeasureUnitId, item.Quantity, corpClientId);
            item.Cost = prodSummary.ProductionItems.Any() ? prodSummary.ProductionCost : new decimal?();
            item.Profit = prodSummary.ProductionItems.Any() ? (item.PriceAfterDiscount - prodSummary.ProductionCost) : new decimal?();
        }
        public async Task ChangeStatus(UpdateOrderStatus model, int corpClientId)
        {
            await orderApp.ChangeStatus(model, corpClientId);
            await orderItemApp.UpdateAllOrderItemStatus(model.OrderNumber, OrderItemStatusEnum.Cancelado, corpClientId);
        }

        public async Task<Order> Get(long orderId, int corpClientId)
        {
            return await orderApp.Get(orderId, corpClientId);
        }

        public async Task<Order> GetByNumber(long orderNumber, int corpClientId)
        {
            return await orderApp.GetByNumber(orderNumber, corpClientId);
        }
        public async Task AddPayment(PayOrder model, int corpClientId, long userId)
        {
            if (model.Amount <= 0)
                throw new BusinessException("O valor do pagamento deve ser maior que zero");

            var currentOrder = await GetByNumber(model.OrderNumber, corpClientId);
            var payments = await transactionApp.GetTransactions(currentOrder.OrderId, corpClientId);
            var totalPaid = payments.Where(p => (p.IsOrderPrincipalAmount ?? false)).Sum(s => s.Amount);
            if (totalPaid + model.Amount > currentOrder.ItemsTotalAfterDiscounts)
                throw new BusinessException("O valor total pago não pode ser maior que o total da venda");

            var nextPaymentStatus = (totalPaid + model.Amount == currentOrder.ItemsTotalAfterDiscounts) ?
                PaymentStatusEnum.Pago : PaymentStatusEnum.ParcialmentePago;

            using var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await orderApp.Update(new UpdateOrder
            {
                CompleteBy = currentOrder.CompleteBy,
                FreightPrice = currentOrder.FreightPrice,
                OrderNumber = currentOrder.OrderNumber,
                PaymentStatusId = nextPaymentStatus
            }, corpClientId);

            await transactionApp.AddTransaction(new NewTransaction
            {
                Amount = model.Amount,
                CorpClientId = corpClientId,
                Date = DateTime.Now,
                Description = $"Recebimento referente ao Pedido #{currentOrder.OrderNumber}",
                MerchantName = currentOrder.Customer.Name,
                OrderId = currentOrder.OrderId,
                TransactionTypeId = model.OrderPaymentMethod,
                TransactionStatusId = 1,
                IsOrderPrincipalAmount = true
            }, userId);
            if (model.Tip > 0)
            {
                await transactionApp.AddTransaction(new NewTransaction
                {
                    Amount = model.Tip,
                    CorpClientId = corpClientId,
                    Date = DateTime.Now,
                    Description = $"Recebimento de Tips referente ao Pedido #{currentOrder.OrderNumber}",
                    MerchantName = currentOrder.Customer.Name,
                    OrderId = currentOrder.OrderId,
                    TransactionTypeId = model.TipPaymentMethod,
                    TransactionStatusId = 1,
                    IsOrderPrincipalAmount = false
                }, userId);
            }
            trans.Complete();
        }
        public async Task RefundPayment(RefundPayment model, int corpClientId, long userId)
        {

            var currentOrder = await GetByNumber(model.OrderNumber, corpClientId);
            var payments = await transactionApp.GetTransactions(currentOrder.OrderId, corpClientId);
            var transaction = payments.FirstOrDefault(p => p.TransactionId == model.TransactionId);

            if (transaction.TransactionStatusId != 1)
                throw new BusinessException("Não é possível fazer estorno desta transação.");

            var totalPaid = payments.Where(s => s.IsIncome && s.TransactionStatusId == 1).Sum(s => s.Amount);

            var nextPaymentStatus = (PaymentStatusEnum)currentOrder.PaymentStatusId;
            if ((transaction.IsOrderPrincipalAmount ?? false))
            {
                if ((currentOrder.ItemsTotalAfterDiscounts - transaction.Amount) == 0)
                    nextPaymentStatus = PaymentStatusEnum.Estornado;
                else if ((currentOrder.ItemsTotalAfterDiscounts - transaction.Amount) == currentOrder.ItemsTotalAfterDiscounts)
                    nextPaymentStatus = PaymentStatusEnum.Pago;
                else if (totalPaid - transaction.Amount <= 0)
                    nextPaymentStatus = PaymentStatusEnum.Pendente;
                else
                    nextPaymentStatus = PaymentStatusEnum.ParcialmentePago;
            }

            using var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await orderApp.Update(new UpdateOrder
            {
                CompleteBy = currentOrder.CompleteBy,
                FreightPrice = currentOrder.FreightPrice,
                OrderNumber = currentOrder.OrderNumber,
                PaymentStatusId = nextPaymentStatus
            }, corpClientId);
            await transactionApp.ChangeStatus(transaction.TransactionId, corpClientId, 2);
            await transactionApp.AddTransaction(new NewTransaction
            {
                Amount = transaction.Amount,
                CorpClientId = corpClientId,
                Date = DateTime.Now,
                Description = $"Transação (#{transaction.TransactionId}) do pedido #{currentOrder.OrderNumber} estornada",
                MerchantName = transaction.MerchantName,
                OrderId = transaction.OrderId,
                TransactionTypeId = 9,
                TransactionStatusId = 1,
                IsOrderPrincipalAmount = transaction.IsOrderPrincipalAmount
            }, userId);
            trans.Complete();
        }

        public async Task Update(UpdateOrder model, int corpClientId)
        {
            await orderApp.Update(model, corpClientId);
        }
        public async Task<OrderStatusEnum> RestoreOrderStatus(long orderNumber, int corpClientId)
        {
            var status = await orderApp.GetCurrentStatus(orderNumber, corpClientId);
            if (status != OrderStatusEnum.Cancelado) return status;
            var nextStatus = await GetNextOrderStatus(orderNumber, corpClientId);
            await orderApp.ChangeStatus(new UpdateOrderStatus
            {
                OrderNumber = orderNumber,
                OrderStatusId = nextStatus
            }, corpClientId);
            return nextStatus;
        }
        private async Task<OrderStatusEnum> GetNextOrderStatus(long orderNumber, int corpClientId)
        {
            var nextStatus = OrderStatusEnum.Criado;
            var orderItems = await orderItemApp.GetAll(orderNumber, corpClientId);
            if (!orderItems.Any()) return nextStatus;
            if (orderItems.All(i => i.OrderItemStatusId == OrderItemStatusEnum.AguardandoProducao))
                nextStatus = OrderStatusEnum.Criado;
            else if (orderItems.All(i => i.OrderItemStatusId == OrderItemStatusEnum.Entregue || i.OrderItemStatusId == OrderItemStatusEnum.Cancelado))
                nextStatus = OrderStatusEnum.Finalizado;
            else if (orderItems.All(i => i.OrderItemStatusId == OrderItemStatusEnum.ProntoParaEntrega || i.OrderItemStatusId == OrderItemStatusEnum.Cancelado || i.OrderItemStatusId == OrderItemStatusEnum.Entregue))
                nextStatus = OrderStatusEnum.AguardandoEntrega;
            else if (orderItems.Any(i => i.OrderItemStatusId == OrderItemStatusEnum.EmAndamento
            || i.OrderItemStatusId == OrderItemStatusEnum.ProntoParaEntrega
            || i.OrderItemStatusId == OrderItemStatusEnum.Entregue))
                nextStatus = OrderStatusEnum.EmAndamento;
            return nextStatus;
        }

        public async Task RemoveOrderItem(long orderId, long orderItemId, int corpClientId)
        {
            await orderItemApp.Remove(orderId, orderItemId, corpClientId);
            var order = await orderApp.Get(orderId, corpClientId);
            var nextStatus = await GetNextOrderStatus(order.OrderNumber, corpClientId);
            await orderApp.ChangeStatus(new UpdateOrderStatus
            {
                OrderNumber = order.OrderNumber,
                OrderStatusId = nextStatus
            }, corpClientId);
        }

        public async Task UpdateOrderItem(UpdateOrderItem model, int corpClientId)
        {
            var item = await orderItemApp.Get(model.OrderItemId, corpClientId);
            var prod = await productApp.Get(corpClientId, model.ProductId);
            model.ProductPrice = item.ProductPrice;
            var units = await measureUnitApp.GetAll();
            var pType = units.FirstOrDefault(u => u.MeasureUnitId == prod.MeasureUnitId).MeasureUnitTypeId;
            var qType = units.FirstOrDefault(u => u.MeasureUnitId == model.MeasureUnitId).MeasureUnitTypeId;
            if (model.OrderItemStatusId == OrderItemStatusEnum.Cancelado)
            {
                model.Cost = null;
                model.Profit = null;
            }
            else
                await CalculateItem(model, prod.MeasureUnitId, pType, qType, corpClientId);

            await orderItemApp.Update(model, corpClientId);
            var nextStatus = await GetNextOrderStatus(item.Order.OrderNumber, corpClientId);
            await orderApp.ChangeStatus(new UpdateOrderStatus
            {
                OrderNumber = item.Order.OrderNumber,
                OrderStatusId = nextStatus
            }, corpClientId);
        }

        public async Task<long> AddOrderItem(NewOrderItem model, int corpClientId)
        {
            var order = await orderApp.Get(model.OrderId, corpClientId);
            var prod = await productApp.Get(corpClientId, model.ProductId);
            model.ProductPrice = prod.Price;
            model.ItemNumber = await orderItemApp.GetLastItemNumber(order.OrderNumber, corpClientId) + 1;
            var units = await measureUnitApp.GetAll();
            var pType = units.FirstOrDefault(u => u.MeasureUnitId == prod.MeasureUnitId).MeasureUnitTypeId;
            var qType = units.FirstOrDefault(u => u.MeasureUnitId == model.MeasureUnitId).MeasureUnitTypeId;
            await CalculateItem(model, prod.MeasureUnitId, pType, qType, corpClientId);
            var result = await orderItemApp.AddOrderItem(model, corpClientId);
            var nextStatus = await GetNextOrderStatus(order.OrderNumber, corpClientId);
            await orderApp.ChangeStatus(new UpdateOrderStatus
            {
                OrderNumber = order.OrderNumber,
                OrderStatusId = nextStatus
            }, corpClientId);
            return result;
        }

        public PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? paidOnFrom, DateTime? paidOnTo, DateTime? completeByFrom, DateTime? completeByTo, List<int> paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return orderApp.GetOrderSummary(corpClientId, customer, createdOnFrom, createdOnTo, paidOnFrom, paidOnTo, completeByFrom, completeByTo, paymentStatus, orderStatus, orderBy, direction, page, pageSize);
        }

        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, List<long> productIds, int massUnitId, int volumeUnitId, List<OrderStatusEnum> orderStatus, List<OrderItemStatusEnum> itemStatus, DateTime? completeByFrom, DateTime? completeByTo, string customer, long? customerId, OrderItemReportOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return orderApp.GetOrderItemReport(corpClientId, orderNumber, productIds, massUnitId, volumeUnitId, orderStatus, itemStatus, completeByFrom, completeByTo, customer, customerId, orderBy, direction, page, pageSize);
        }
        public async Task CloseOrder(long orderNumber, int corpClientId)
        {
            await orderItemApp.UpdateAllOrderItemStatus(orderNumber, OrderItemStatusEnum.Entregue, corpClientId);
            var nextStatus = await GetNextOrderStatus(orderNumber, corpClientId);
            await orderApp.ChangeStatus(new UpdateOrderStatus
            {
                OrderNumber = orderNumber,
                OrderStatusId = nextStatus
            }, corpClientId);
        }

        public async Task<OrderCountSummary> GetOrderCountSummary(int corpClientId)
        {
            return await orderApp.GetOrderCountSummary(corpClientId);
        }

        public async Task<ProfitSummary> GetProfitSummary(int corpClientId)
        {
            return await orderApp.GetProfitSummary(corpClientId);
        }

        public async Task<SalesSummary> GetSalesSummary(int corpClientId)
        {
            return await orderApp.GetSalesSummary(corpClientId);
        }

        public async Task<PendingPaymentsSummary> GetPendingPaymentsSummary(int corpClientId)
        {
            return await orderApp.GetPendingPaymentsSummary(corpClientId);
        }

        public async Task<IEnumerable<SalesPerMonth>> GetSalesPerMonth(int corpClientId)
        {
            return await orderApp.GetSalesPerMonth(corpClientId);
        }
        public PagedResult<SummarizedOrderReport> GetSummarizedReport(int corpClientId, int volumeUnitId, int massUnitId, List<OrderItemStatusEnum> itemStatus, List<long> productIds, SummarizedOrderOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return orderApp.GetSummarizedReport(corpClientId, volumeUnitId, massUnitId, itemStatus, productIds, orderBy, direction, page, pageSize);
        }
    }
}
