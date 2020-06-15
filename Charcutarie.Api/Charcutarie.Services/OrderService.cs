using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public OrderService(IOrderApp orderApp, IPricingApp pricingApp, IProductApp productApp, IOrderItemApp orderItemApp, IMeasureUnitApp measureUnitApp, IDataSheetService dataSheetService)
        {
            this.orderApp = orderApp;
            this.pricingApp = pricingApp;
            this.productApp = productApp;
            this.orderItemApp = orderItemApp;
            this.measureUnitApp = measureUnitApp;
            this.dataSheetService = dataSheetService;
        }
        public async Task<int> Add(NewOrder model, int corpClientId)
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
            item.Cost = prodSummary.ProductionItems.Any() ? prodSummary.ProductionCost : new double?();
            item.Profit = prodSummary.ProductionItems.Any() ? (item.PriceAfterDiscount - prodSummary.ProductionCost) : new double?();
        }
        public async Task ChangeStatus(UpdateOrderStatus model, int corpClientId)
        {
            await orderApp.ChangeStatus(model, corpClientId);
        }

        public async Task<Order> Get(long orderId, int corpClientId)
        {
            return await orderApp.Get(orderId, corpClientId);
        }

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            return await orderApp.GetByNumber(orderNumber, corpClientId);
        }

        public async Task Update(UpdateOrder model, int corpClientId)
        {
            await orderApp.Update(model, corpClientId);
        }
        public async Task<OrderStatusEnum> RestoreOrderStatus(int orderNumber, int corpClientId)
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
        private async Task<OrderStatusEnum> GetNextOrderStatus(int orderNumber, int corpClientId)
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
                await CalculateItem(model as NewOrderItem, prod.MeasureUnitId, pType, qType, corpClientId);

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

        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, List<OrderStatusEnum> orderStatus, List<OrderItemStatusEnum> itemStatus, DateTime? completeByFrom, DateTime? completeByTo, string customer, OrderItemReportOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return orderApp.GetOrderItemReport(corpClientId, orderNumber, orderStatus, itemStatus, completeByFrom, completeByTo, customer, orderBy, direction, page, pageSize);
        }
        public async Task CloseOrder(int orderNumber, int corpClientId)
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
    }
}
