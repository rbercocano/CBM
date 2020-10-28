using AutoMapper;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Data;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models;
using Charcutarie.Models.Enums;
using System.IO;

namespace Charcutarie.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public OrderRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<long> Add(NewOrder model, int corpClientId)
        {
            var lastNumber = context.Orders.Where(o => o.Customer.CorpClientId == corpClientId).Max(o => (int?)o.OrderNumber) ?? 0;
            model.OrderNumber = lastNumber + 1;

            var entity = mapper.Map<EF.Order>(model);
            entity.OrderItems.ForEach(i => i.LastStatusDate = DateTimeOffset.UtcNow);
            if (model.PaymentStatusId == PaymentStatusEnum.Pago)
                entity.PaidOn = DateTimeOffset.UtcNow;
            context.Orders.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.OrderNumber);
        }
        public async Task Update(UpdateOrder model, int corpClientId)
        {
            var entity = await context.Orders.FirstOrDefaultAsync(o => o.Customer.CorpClientId == corpClientId && o.OrderNumber == model.OrderNumber);

            if (entity.PaymentStatusId != PaymentStatusEnum.Pago &&
                (model.PaymentStatusId == PaymentStatusEnum.Pago || model.PaymentStatusId == PaymentStatusEnum.ParcialmentePago))
                entity.PaidOn = DateTimeOffset.UtcNow;
            else if ((entity.PaymentStatusId == PaymentStatusEnum.Pago || entity.PaymentStatusId == PaymentStatusEnum.ParcialmentePago)
                && model.PaymentStatusId != PaymentStatusEnum.Pendente)
                entity.PaidOn = null;

            entity.CompleteBy = model.CompleteBy;
            entity.FreightPrice = model.FreightPrice;
            entity.PaymentStatusId = model.PaymentStatusId;
            context.Orders.Update(entity);

            var rows = await context.SaveChangesAsync();
        }
        public async Task ChangeStatus(UpdateOrderStatus model, int corpClientId)
        {
            var entity = await context.Orders.FirstOrDefaultAsync(o => o.Customer.CorpClientId == corpClientId && o.OrderNumber == model.OrderNumber);
            entity.OrderStatusId = model.OrderStatusId;
            context.Orders.Update(entity);
            var rows = await context.SaveChangesAsync();
        }
        public async Task<Order> Get(long orderId, int corpClientId)
        {
            var data = await context.Orders.Where(o => o.OrderId == orderId
                                             && o.Customer.CorpClientId == corpClientId)
                                            .Include(o => o.Customer)
                                                .ThenInclude(o => o.Contacts)
                                                    .ThenInclude(o => o.ContactType)
                                            .Include(o => o.OrderStatus)
                                            .Include(o => o.PaymentStatus)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.OrderItemStatus)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.Product)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.MeasureUnit)
                                                .ToListAsync();
            var result = mapper.Map<Order>(data.FirstOrDefault());
            return result;
        }

        public async Task<Order> GetByNumber(long orderNumber, int corpClientId)
        {
            var data = await context.Orders.Where(o => o.OrderNumber == orderNumber
                                             && o.Customer.CorpClientId == corpClientId)
                                            .Include(o => o.Transactions)
                                                .ThenInclude(o => o.TransactionType)
                                            .Include(o => o.Customer)
                                                .ThenInclude(o => o.Contacts)
                                                    .ThenInclude(o => o.ContactType)
                                            .Include(o => o.OrderStatus)
                                            .Include(o => o.PaymentStatus)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.OrderItemStatus)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.Product)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(o => o.MeasureUnit)
                                                .ToListAsync();
            var result = mapper.Map<Order>(data.FirstOrDefault());
            return result;
        }
        public async Task<OrderStatusEnum> GetCurrentStatus(long orderNumber, int corpClientId)
        {
            return await context.Orders.Where(o => o.OrderNumber == orderNumber && o.Customer.CorpClientId == corpClientId).Select(o => o.OrderStatusId).FirstOrDefaultAsync();
        }
        public PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTime? createdOnFrom, DateTime? createdOnTo,
                                                         DateTime? paidOnFrom, DateTime? paidOnTo,
                                                         DateTime? completeByFrom, DateTime? completeByTo,
                                                          List<int> paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction,
                                                         int? page, int? pageSize)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId)
            };
            var query = new StringBuilder(@"Select * FROM (SELECT C.CorpClientId,
	                        O.OrderId,
	                        O.OrderNumber,
	                        CASE C.CustomerTypeId WHEN 1 THEN RTRIM(c.Name + ' ' + ISNULL(c.LastName,'')) ELSE c.DBAName END AS Name,
	                        CASE C.CustomerTypeId WHEN 1 THEN c.CPF ELSE c.CNPJ END AS SocialIdentifier,
	                        P.PaymentStatusId,
	                        OS.OrderStatusId,
                            C.CustomerTypeId,   
                            C.CustomerId,
	                        P.Description AS PaymentStatus,
	                        OS.Description AS OrderStatus,
	                        O.CompleteBy CompleteBy,
	                        O.CreatedOn CreatedOn,
	                        O.PaidOn PaidOn,
	                        ISNULL((SELECT SUM(OI.PriceAfterDiscount) FROM OrderItem OI WHERE OI.OrderId = O.OrderId AND OI.OrderItemStatusId NOT IN (3)),0)  AS FinalPrice
                        FROM Customer C 
                        JOIN [Order] O ON C.CustomerId = O.CustomerId
                        JOIN PaymentStatus P ON O.PaymentStatusId = P.PaymentStatusId
                        JOIN OrderStatus OS ON O.OrderStatusId = OS.OrderStatusId
                        ) X WHERE CorpClientId = @corpClientId ");
            if (!string.IsNullOrEmpty(customer))
            {
                query.Append(" AND Name LIKE '%'+ @customer +'%'");
                sqlParams.Add(new SqlParameter("@customer", customer) { SqlDbType = SqlDbType.VarChar, Size = 500 });
            }
            if (createdOnFrom.HasValue)
            {
                query.Append(" AND CreatedOn >= @createdOnFrom");
                sqlParams.Add(new SqlParameter("@createdOnFrom", createdOnFrom) { SqlDbType = SqlDbType.Date });
            }
            if (createdOnTo.HasValue)
            {
                query.Append(" AND CreatedOn <= @createdOnTo");
                sqlParams.Add(new SqlParameter("@createdOnTo", createdOnTo) { SqlDbType = SqlDbType.Date });
            }
            if (paidOnFrom.HasValue)
            {
                query.Append(" AND PaidOn >= @paidOnFrom");
                sqlParams.Add(new SqlParameter("@paidOnFrom", paidOnFrom) { SqlDbType = SqlDbType.Date });
            }
            if (paidOnTo.HasValue)
            {
                query.Append(" AND PaidOn <= @paidOnTo");
                sqlParams.Add(new SqlParameter("@paidOnTo", paidOnTo) { SqlDbType = SqlDbType.Date });
            }
            if (completeByFrom.HasValue)
            {
                query.Append(" AND CompleteBy >= @completeByFrom");
                sqlParams.Add(new SqlParameter("@completeByFrom", completeByFrom) { SqlDbType = SqlDbType.Date });
            }
            if (completeByTo.HasValue)
            {
                query.Append(" AND CompleteBy <= @completeByTo");
                sqlParams.Add(new SqlParameter("@completeByTo", completeByTo) { SqlDbType = SqlDbType.Date });
            }
            if (paymentStatus != null && paymentStatus.Any())
                query.Append($" AND paymentStatusId IN ({string.Join(',', paymentStatus.ToArray())})");

            if (orderStatus != null && orderStatus.Any())
                query.Append($" AND orderStatusId IN ({string.Join(',', orderStatus.ToArray())})");
            var count = context.OrderSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).Count();
            if (orderBy == OrderSummaryOrderBy.OrderStatus)
                query.Append($" ORDER BY {orderBy} {direction}, CompleteBy");
            else if (orderBy == OrderSummaryOrderBy.CompleteBy)
                query.Append($" ORDER BY {orderBy} {direction}, OrderStatus");
            else
                query.Append($" ORDER BY {orderBy} {direction},CompleteBy, OrderStatus");

            if (page.HasValue && pageSize.HasValue)
                query.Append($" OFFSET {(page > 0 ? page - 1 : 0) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            var data = context.OrderSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).ToList();
            var result = mapper.Map<IEnumerable<OrderSummary>>(data);
            return new PagedResult<OrderSummary>
            {
                CurrentPage = page ?? 1,
                Data = result,
                RecordCount = count,
                RecordsPerpage = pageSize ?? count
            };
        }
        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, List<long> productIds, int massUnitId, int volumeUnitId, List<OrderStatusEnum> orderStatus, List<OrderItemStatusEnum> itemStatus, DateTime? completeByFrom, DateTime? completeByTo,
                                                               string customer, long? customerId, OrderItemReportOrderBy orderBy, OrderByDirection direction,
                                                               int? page, int? pageSize)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId),
                new SqlParameter("@mid", massUnitId),
                new SqlParameter("@vid", volumeUnitId)
            };
            var query = new StringBuilder(@"SELECT * FROM(SELECT 
	                                        O.OrderId,
                                            I.OrderItemId,
	                                        O.OrderNumber,
                                            C.customerTypeId,
	                                        OS.OrderStatusId AS OrderStatusId,
	                                        OS.Description AS OrderStatus,
	                                        CASE C.CustomerTypeId WHEN 1 THEN RTRIM(c.Name + ' ' + ISNULL(c.LastName,'')) ELSE c.DBAName END AS Customer,
	                                        CASE C.CustomerTypeId WHEN 1 THEN c.CPF ELSE c.CNPJ END AS SocialIdentifier,
	                                        I.ItemNumber,
	                                        P.Name as Product,
	                                        OIS.Description as OrderItemStatus,
	                                        OIS.OrderItemStatusId as OrderItemStatusId,
	                                        I.PriceAfterDiscount as FinalPrice,
	                                        I.LastStatusDate,
	                                        O.CompleteBy,
                                            P.ProductId, 
                                            C.CustomerId,
	                                        CASE 
		                                        WHEN M.MeasureUnitTypeId = 1 THEN
			                                        CAST(dbo.ConvertMeasure(M.MeasureUnitId,I.Quantity,@mid) as decimal(18,2))
		                                        ELSE
			                                        CAST(dbo.ConvertMeasure(M.MeasureUnitId,I.Quantity,@vid) as decimal(18,2))
	                                        END as Quantity,
	                                        (SELECT M2.ShortName FROM MeasureUnit M2 WHERE M2.MeasureUnitId = IIF(M.MeasureUnitTypeId=1,@mid,@vid)) as MeasureUnit
 
                                        FROM [Order] O
                                        JOIN OrderStatus OS ON O.OrderStatusId = OS.OrderStatusId
                                        JOIN PaymentStatus PS ON O.PaymentStatusId = PS.PaymentStatusId
                                        JOIN OrderItem I ON O.OrderId = I.OrderId
                                        JOIN Customer C ON C.CustomerId = O.CustomerId
                                        JOIN OrderItemStatus OIS ON I.OrderItemStatusId = OIS.OrderItemStatusId
                                        JOIN MeasureUnit M ON I.MeasureUnitId = M.MeasureUnitId
                                        JOIN Product P ON I.ProductId = P.ProductId
                                        WHERE P.CorpClientId = @corpClientId AND C.CorpClientId = @corpClientId) X WHERE 1 = 1 ");

            if (productIds != null && productIds.Any())
                query.Append($" AND ProductId IN ({string.Join(',', productIds.Select(i => i).ToArray())})");

            if (customerId.HasValue)
            {
                query.Append($" AND CustomerId = @customerId");
                sqlParams.Add(new SqlParameter("@customerId", customerId) { SqlDbType = SqlDbType.BigInt });
            }
            if (completeByFrom.HasValue)
            {
                query.Append(" AND CompleteBy >= @completeByFrom");
                sqlParams.Add(new SqlParameter("@completeByFrom", completeByFrom) { SqlDbType = SqlDbType.Date });
            }
            if (completeByTo.HasValue)
            {
                query.Append(" AND CompleteBy <= @completeByTo");
                sqlParams.Add(new SqlParameter("@completeByTo", completeByTo) { SqlDbType = SqlDbType.Date });
            }
            if (!string.IsNullOrEmpty(customer))
            {
                query.Append(" AND Customer LIKE '%'+ @customer +'%'");
                sqlParams.Add(new SqlParameter("@customer", customer) { SqlDbType = SqlDbType.VarChar, Size = 500 });
            }
            if (itemStatus != null && itemStatus.Any())
                query.Append($" AND OrderItemStatusId IN ({string.Join(',', itemStatus.Select(i => (int)i).ToArray())})");


            if (orderNumber.HasValue)
            {
                query.Append($" AND OrderNumber = @OrderNumber");
                sqlParams.Add(new SqlParameter("@OrderNumber", orderNumber.Value) { SqlDbType = SqlDbType.Int });
            }

            if (orderStatus != null && orderStatus.Any())
                query.Append($" AND orderStatusId IN ({string.Join(',', orderStatus.Select(i => (int)i).ToArray())})");


            var count = context.OrderItemReports.FromSqlRaw(query.ToString(), sqlParams.ToArray()).Count();
            if (orderBy == OrderItemReportOrderBy.OrderItemStatus)
                query.Append($" ORDER BY {orderBy} {direction}, Product ASC");
            else if (orderBy == OrderItemReportOrderBy.Product)
                query.Append($" ORDER BY {orderBy} {direction}, OrderItemStatus ASC");
            else
                query.Append($" ORDER BY {orderBy} {direction}, OrderItemStatus ASC, Product ASC");

            if (page.HasValue && pageSize.HasValue)
                query.Append($" OFFSET {(page > 0 ? page - 1 : 0) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            var data = context.OrderItemReports.FromSqlRaw(query.ToString(), sqlParams.ToArray()).ToList();
            var result = mapper.Map<IEnumerable<OrderItemReport>>(data);
            return new PagedResult<OrderItemReport>
            {
                CurrentPage = page ?? 1,
                Data = result,
                RecordCount = count,
                RecordsPerpage = pageSize ?? count
            };
        }
        public async Task<PendingPaymentsSummary> GetPendingPaymentsSummary(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@corpClientId", corpClientId));
            var query = new StringBuilder(@"SELECT NewId() AS RowId, ISNULL(X.OrderTotals,0) - ISNULL(X.TotalPaid,0) AS TotalPendingPayments ,
                                                   ISNULL(X.FinishedTotals,0) - ISNULL(X.FinishedPaid,0) AS FinishedOrdersPendingPayment
                                            FROM 
	                                            (SELECT 
		                                            (SELECT 
		                                            ISNULL(SUM(T.Amount),0) AS TotalPaid
		                                            FROM [Transaction] T 
		                                            JOIN [Order] O ON T.OrderId = O.OrderId
		                                            JOIN [OrderItem] I ON I.OrderId = O.OrderId
		                                            JOIN Product P ON I.ProductId = P.ProductId 
		                                            JOIN TransactionType TT  ON T.TransactionTypeId = TT.TransactionTypeId
		                                            WHERE P.CorpClientId = @corpClientId
		                                            AND O.OrderStatusId NOT IN (4)
		                                            AND T.IsOrderPrincipalAmount = 1
		                                            AND O.PaymentStatusId  IN (1,3)) AS TotalPaid,
		                                            (SELECT 
			                                            ISNULL(  SUM(I.PriceAfterDiscount) ,0)
		                                            FROM OrderItem I 
		                                            JOIN Product P ON I.ProductId = P.ProductId 
		                                            JOIN [Order] O ON I.OrderId = O.OrderId
		                                            WHere P.CorpClientId = @corpClientId
		                                            AND O.OrderStatusId NOT IN (4)
		                                            AND O.PaymentStatusId  IN (1,3)
		                                            AND O.OrderStatusId NOT IN (4)) As OrderTotals,
		                                            (SELECT 
			                                            ISNULL(  SUM(I.PriceAfterDiscount) ,0)
		                                            FROM OrderItem I 
		                                            JOIN Product P ON I.ProductId = P.ProductId 
		                                            JOIN [Order] O ON I.OrderId = O.OrderId
		                                            WHere P.CorpClientId = @corpClientId
		                                            AND O.OrderStatusId NOT IN (4)
		                                            AND O.PaymentStatusId  IN (1,3)
		                                            AND O.OrderStatusId IN (3,5)) As FinishedTotals,
		                                            (SELECT 
		                                            ISNULL(SUM(T.Amount),0) AS TotalPaid
		                                            FROM [Transaction] T 
		                                            JOIN [Order] O ON T.OrderId = O.OrderId
		                                            JOIN [OrderItem] I ON I.OrderId = O.OrderId
		                                            JOIN Product P ON I.ProductId = P.ProductId 
		                                            JOIN TransactionType TT  ON T.TransactionTypeId = TT.TransactionTypeId
		                                            WHERE P.CorpClientId = @corpClientId
		                                            AND O.OrderStatusId IN (3,5)
		                                            AND T.IsOrderPrincipalAmount = 1
		                                            AND O.PaymentStatusId  IN (1,3)) AS FinishedPaid) X");
            var data = await context.PendingPaymentsSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).FirstOrDefaultAsync();
            return mapper.Map<PendingPaymentsSummary>(data);
        }
        public async Task<SalesSummary> GetSalesSummary(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId),
                new SqlParameter("@start", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date) { SqlDbType = SqlDbType.Date },
                new SqlParameter("@end", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Date)
                { SqlDbType = SqlDbType.Date }
            };
            var query = new StringBuilder(@"SELECT 
	                                            NEWID() AS RowId,
                                            (SELECT 
	                                           ISNULL( SUM(I.PriceAfterDiscount),0)
                                            FROM OrderItem I 
                                            JOIN [Order] O On O.OrderId = I.OrderId
                                            JOIN OrderItemStatus S ON I.OrderItemStatusId = S.OrderItemStatusId
                                            JOIN Product P ON I.ProductId = P.ProductId 
                                            WHere P.CorpClientId = @corpClientId
                                            AND OrderStatusId <> 4) AS TotalSales,

                                            (SELECT 
	                                            ISNULL(SUM(I.PriceAfterDiscount) ,0)
                                            FROM OrderItem I 
                                            JOIN [Order] O On O.OrderId = I.OrderId
                                            JOIN Product P ON I.ProductId = P.ProductId 
                                            WHere P.CorpClientId = @corpClientId
                                            AND CAST(O.CreatedOn AS DATE) Between @start AND  @end
                                            AND OrderStatusId <> 4) AS CurrentMonthSales
                                            ");
            var data = await context.SalesSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).FirstOrDefaultAsync();
            return mapper.Map<SalesSummary>(data);
        }

        public async Task<ProfitSummary> GetProfitSummary(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId),
                new SqlParameter("@start", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date) { SqlDbType = SqlDbType.Date },
                new SqlParameter("@end", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Date)
                { SqlDbType = SqlDbType.Date }
            };
            var query = new StringBuilder(@"SELECT 
	                                            NEWID() AS RowId,
                                            (SELECT 
	                                            ISNULL(SUM(I.Profit) ,0)
                                            FROM OrderItem I 
                                            JOIN [Order] O On O.OrderId = I.OrderId
                                            JOIN OrderItemStatus S ON I.OrderItemStatusId = S.OrderItemStatusId
                                            JOIN Product P ON I.ProductId = P.ProductId 
                                            WHere P.CorpClientId = @corpClientId
                                            AND OrderStatusId <> 4) AS TotalProfit,

                                            (SELECT 
	                                             ISNULL(SUM(I.Profit) ,0)
                                            FROM OrderItem I 
                                            JOIN [Order] O On O.OrderId = I.OrderId
                                            JOIN Product P ON I.ProductId = P.ProductId 
                                            WHere P.CorpClientId = @corpClientId
                                            AND CAST(O.CreatedOn AS DATE) Between @start AND  @end
                                            AND OrderStatusId <> 4) AS CurrentMonthProfit
                                            ");
            var data = await context.ProfitSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).FirstOrDefaultAsync();
            return mapper.Map<ProfitSummary>(data);
        }

        public async Task<OrderCountSummary> GetOrderCountSummary(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId)
            };
            var query = new StringBuilder(@"SELECT 
	                                            NEWID() AS RowId,
	                                            (SELECT COUNT(*)
                                            FROM [Order] O
                                            JOIN Customer C ON O.CustomerId = C.CustomerId
                                            WHERE C.CorpClientId = @corpClientId)  AS TotalOrders,

                                            (SELECT COUNT(*) 
                                            FROM [Order] O
                                            JOIN Customer C ON O.CustomerId = C.CustomerId
                                            WHERE C.CorpClientId = @corpClientId AND OrderStatusId = 3) AS TotalCompletedOrders");
            var data = await context.OrderCountSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).FirstOrDefaultAsync();
            return mapper.Map<OrderCountSummary>(data);
        }


        public async Task<IEnumerable<SalesPerMonth>> GetSalesPerMonth(int corpClientId)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId)
            };
            var query = new StringBuilder(@"SELECT TOP 6
	                                            CAST(CAST(YEAR(O.CreatedOn) AS VARCHAR(4)) + '-'+CAST(MONTH(O.CreatedOn) AS VARCHAR(2))+'-01' AS DATE) AS [Date],
	                                            SUM(I.PriceAfterDiscount) AS TotalSales,
	                                            SUM(I.Profit) AS TotalProfit
                                            FROM OrderItem I 
                                            JOIN Product P ON I.ProductId = P.ProductId
                                            JOIN [ORDER] O ON O.OrderId = I.OrderId
                                            WHERE 
	                                            P.CorpClientId = @corpClientId 
	                                            AND I.OrderItemStatusId <> 3
                                            GROUP BY 
	                                            CAST(CAST(YEAR(O.CreatedOn) AS VARCHAR(4)) + '-'+CAST(MONTH(O.CreatedOn) AS VARCHAR(2))+'-01' AS DATE)
                                            ORDER BY 1 DESC");
            var data = await context.SalesPerMonths.FromSqlRaw(query.ToString(), sqlParams.ToArray()).ToListAsync();
            var range = new[] { DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddMonths(-1),
                DateTimeOffset.UtcNow.AddMonths(-2),
                DateTimeOffset.UtcNow.AddMonths(-3),
                DateTimeOffset.UtcNow.AddMonths(-4),
                DateTimeOffset.UtcNow.AddMonths(-5) }.ToList();
            var result = mapper.Map<IEnumerable<SalesPerMonth>>(data);
            result = (from d in range
                      join r in result on d.ToString("yyyy/MM") equals r.Date.ToString("yyyy/MM") into gj
                      from s in gj.DefaultIfEmpty()
                      select new SalesPerMonth
                      {
                          Date = new DateTime(d.Year, d.Month, 1),
                          TotalProfit = s?.TotalProfit ?? 0,
                          TotalSales = s?.TotalSales ?? 0
                      }).ToList();
            return result.OrderBy(o => o.Date);
        }

        public PagedResult<SummarizedOrderReport> GetSummarizedReport(int corpClientId, int volumeUnitId, int massUnitId,
                                                                List<OrderItemStatusEnum> itemStatus,
                                                                List<long> productIds,
                                                                SummarizedOrderOrderBy orderBy, OrderByDirection direction,
                                                                int? page, int? pageSize)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@corpClientId", corpClientId),
                new SqlParameter("@mid", massUnitId),
                new SqlParameter("@vid", volumeUnitId)
            };
            var query = new StringBuilder(@"SELECT * FROM(SELECT 
	                                        NEWID() as RowId,
	                                        ProductId, 
	                                        Product,
	                                        OrderItemStatusId,
	                                        OrderItemStatus,
                                            MeasureUnitTypeId,
	                                        SUM(Quantity) as Quantity,
	                                        CASE 
                                                WHEN MeasureUnitTypeId = 1 THEN @mid
											    ELSE @vid 
                                            END as MeasureUnitId,
	                                        (SELECT M.Description FROM MeasureUnit M WHERE M.MeasureUnitId = IIF(X.MeasureUnitTypeId=1,@mid,@vid)) as MeasureUnit,
	                                        (SELECT M.ShortName FROM MeasureUnit M WHERE M.MeasureUnitId = IIF(X.MeasureUnitTypeId=1,@mid,@vid)) as ShortMeasureUnit
                                        FROM 
	                                        (SELECT	
		                                        P.ProductId,
		                                        P.Name as Product,
		                                        CASE 
			                                        WHEN U.MeasureUnitTypeId = 1 THEN
				                                        CAST(dbo.ConvertMeasure(I.MeasureUnitId,I.Quantity,@mid) as decimal(18,2))
			                                        ELSE
				                                        CAST(dbo.ConvertMeasure(I.MeasureUnitId,I.Quantity,@vid) as decimal(18,2))
		                                        END as Quantity,
		                                        CASE 
			                                        WHEN U.MeasureUnitTypeId = 1 THEN 1
			                                        ELSE 5
		                                        END as MeasureUnitId,
		                                        U.MeasureUnitTypeId,
		                                        S.OrderItemStatusId,
		                                        S.Description AS  OrderItemStatus
	                                        FROM OrderItem I
	                                        JOIN Product P ON I.ProductId  = P.ProductId
	                                        JOIN OrderItemStatus S ON I.OrderItemStatusId = S.OrderItemStatusId
	                                        JOIN MeasureUnit U ON I.MeasureUnitId = U.MeasureUnitId
	                                        WHERE P.CorpClientId = @corpClientId) X
                                        GROUP BY	
	                                        ProductId, 
	                                        Product,
	                                        MeasureUnitTypeId,
	                                        OrderItemStatusId,
	                                        OrderItemStatus,
	                                        MeasureUnitId
                                        ) X WHERE 1 = 1 ");

            if (itemStatus != null && itemStatus.Any())
                query.Append($" AND OrderItemStatusId IN ({string.Join(',', itemStatus.Select(i => (int)i).ToArray())})");

            if (productIds != null && productIds.Any())
                query.Append($" AND ProductId IN ({string.Join(',', productIds.Select(i => i).ToArray())})");

            var count = context.SummarizedOrderReports.FromSqlRaw(query.ToString(), sqlParams.ToArray()).Count();
            if (orderBy == SummarizedOrderOrderBy.OrderItemStatus)
                query.Append($" ORDER BY {orderBy} {direction}, Product ASC");
            else if (orderBy == SummarizedOrderOrderBy.Product)
                query.Append($" ORDER BY {orderBy} {direction}, OrderItemStatus ASC");
            else
                query.Append($" ORDER BY {orderBy} {direction}, OrderItemStatus ASC, Product ASC");

            if (page.HasValue && pageSize.HasValue)
                query.Append($" OFFSET {(page > 0 ? page - 1 : 0) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            var data = context.SummarizedOrderReports.FromSqlRaw(query.ToString(), sqlParams.ToArray()).ToList();
            var result = mapper.Map<IEnumerable<SummarizedOrderReport>>(data);
            return new PagedResult<SummarizedOrderReport>
            {
                CurrentPage = page ?? 1,
                Data = result,
                RecordCount = count,
                RecordsPerpage = pageSize ?? count
            };
        }
    }
}