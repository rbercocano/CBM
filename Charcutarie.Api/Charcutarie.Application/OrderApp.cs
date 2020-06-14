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
        public async Task<int> Add(NewOrder model, int corpClientId)
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

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            return await repository.GetByNumber(orderNumber, corpClientId);
        }

        public async Task<OrderStatusEnum> GetCurrentStatus(int orderNumber, int corpClientId)
        {
            return await repository.GetCurrentStatus(orderNumber, corpClientId);
        }

        public async Task<OrderCountSummary> GetOrderCountSummary(int corpClientId)
        {
            return await repository.GetOrderCountSummary(corpClientId);
        }

        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, OrderStatusEnum? orderStatus, OrderItemStatusEnum? itemStatus, DateTime? completeByFrom, DateTime? completeByTo, string customer, OrderItemReportOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
        {
            return repository.GetOrderItemReport(corpClientId, orderNumber, orderStatus, itemStatus, completeByFrom, completeByTo, customer, orderBy, direction, page, pageSize);
        }

        public PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTime? createdOnFrom, DateTime? createdOnTo, DateTime? paidOnFrom, DateTime? paidOnTo, DateTime? completeByFrom, DateTime? completeByTo, int? paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction, int? page, int? pageSize)
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

        public async Task<SalesSummary> GetSalesSummary(int corpClientId)
        {
            return await repository.GetSalesSummary(corpClientId);
        }

        public async Task Update(UpdateOrder model, int corpClientId)
        {
            await repository.Update(model, corpClientId);
        }
    }
}
