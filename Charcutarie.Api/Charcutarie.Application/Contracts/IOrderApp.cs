﻿using Charcutarie.Models;
using Charcutarie.Models.Enums;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IOrderApp
    {
        Task<long> Add(NewOrder model, int corpClientId);
        Task<Order> Get(long orderId, int corpClientId);
        Task<Order> GetByNumber(long orderNumber, int corpClientId);
        Task Update(UpdateOrder model, int corpClientId);
        Task ChangeStatus(UpdateOrderStatus model, int corpClientId);
        Task<OrderStatusEnum> GetCurrentStatus(long orderNumber, int corpClientId);
        PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTimeOffset? createdOnFrom, DateTimeOffset? createdOnTo,
                                                         DateTimeOffset? paidOnFrom, DateTimeOffset? paidOnTo,
                                                         DateTime? completeByFrom, DateTime? completeByTo,
                                                          List<int> paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction,
                                                         int? page, int? pageSize);

        PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, List<long> productIds, int massUnitId, int volumeUnitId, List<OrderStatusEnum> orderStatus, List<OrderItemStatusEnum> itemStatus, DateTime? completeByFrom, DateTime? completeByTo,
                                                               string customer, long? customerId, OrderItemReportOrderBy orderBy, OrderByDirection direction,
                                                               int? page, int? pageSize);

        Task<OrderCountSummary> GetOrderCountSummary(int corpClientId);
        Task<ProfitSummary> GetProfitSummary(int corpClientId);
        Task<SalesSummary> GetSalesSummary(int corpClientId);
        Task<PendingPaymentsSummary> GetPendingPaymentsSummary(int corpClientId);
        Task<IEnumerable<SalesPerMonth>> GetSalesPerMonth(int corpClientId);
        PagedResult<SummarizedOrderReport> GetSummarizedReport(int corpClientId,
                                                               int volumeUnitId, int massUnitId,
                                                               List<OrderItemStatusEnum> itemStatus,
                                                               List<long> productIds,
                                                               SummarizedOrderOrderBy orderBy, OrderByDirection direction,
                                                               int? page, int? pageSize);

    }
}