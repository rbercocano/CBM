using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class OrderApp : IOrderApp
    {
        private readonly IOrderRepository repository;

        public OrderApp(IOrderRepository repository)
        {
            this.repository = repository;
        }
        public async Task<long> Add(NewOrder model, int corpClientId)
        {
            return await repository.Add(model, corpClientId);
        }

        public async Task ChangeStatus(UpdateOrderStatus model, int corpClientId)
        {
            await repository.ChangeStatus(model, corpClientId);
        }

        public async Task<Order> Get(long orderId, int corpClientId)
        {
            return await repository.Get(orderId, corpClientId);
        }

        public async Task<Order> GetByNumber(long orderNumber, int corpClientId)
        {
            return await repository.GetByNumber(orderNumber, corpClientId);
        }

        public async Task<OrderStatusEnum> GetCurrentStatus(long orderNumber, int corpClientId)
        {
            return await repository.GetCurrentStatus(orderNumber, corpClientId);
        }

        public async Task<OrderCountSummary> GetOrderCountSummary(int corpClientId)
        {
            return await repository.GetOrderCountSummary(corpClientId);
        }

        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, List<long> productIds, int massUnitId, int volumeUnitId, List<OrderStatusEnum> orderStatus, List<OrderItemStatusEnum> itemStatus, DateTime? completeByFrom, DateTime? completeByTo, string customer, long? customerId, OrderItemReportOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return repository.GetOrderItemReport(corpClientId, orderNumber, productIds, massUnitId, volumeUnitId, orderStatus, itemStatus, completeByFrom, completeByTo, customer, customerId, orderBy, direction, page, pageSize);
        }

        public PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? paidOnFrom, DateTime? paidOnTo, DateTime? completeByFrom, DateTime? completeByTo, List<int> paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return repository.GetOrderSummary(corpClientId, customer, createdOnFrom, createdOnTo, paidOnFrom, paidOnTo, completeByFrom, completeByTo, paymentStatus, orderStatus, orderBy, direction, page, pageSize);
        }

        public async Task<PendingPaymentsSummary> GetPendingPaymentsSummary(int corpClientId)
        {
            return await repository.GetPendingPaymentsSummary(corpClientId);
        }

        public async Task<ProfitSummary> GetProfitSummary(int corpClientId)
        {
            return await repository.GetProfitSummary(corpClientId);
        }

        public async Task<IEnumerable<SalesPerMonth>> GetSalesPerMonth(int corpClientId)
        {
            return await repository.GetSalesPerMonth(corpClientId);
        }

        public async Task<SalesSummary> GetSalesSummary(int corpClientId)
        {
            return await repository.GetSalesSummary(corpClientId);
        }

        public PagedResult<SummarizedOrderReport> GetSummarizedReport(int corpClientId, int volumeUnitId, int massUnitId, List<OrderItemStatusEnum> itemStatus, List<long> productIds, SummarizedOrderOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return repository.GetSummarizedReport(corpClientId, volumeUnitId, massUnitId, itemStatus, productIds, orderBy, direction, page, pageSize);
        }


        public async Task Update(UpdateOrder model, int corpClientId)
        {
            await repository.Update(model, corpClientId);
        }
    }
}
