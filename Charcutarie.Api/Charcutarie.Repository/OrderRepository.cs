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
        public async Task<int> Add(NewOrder model, int corpClientId)
        {
            var lastNumber = context.Orders.Where(o => o.Customer.CorpClientId == corpClientId).Max(o => (int?)o.OrderNumber) ?? 0;
            model.OrderNumber = lastNumber + 1;

            var entity = mapper.Map<EF.Order>(model);
            if (model.PaymentStatusId == PaymentStatusEnum.Pago)
                entity.PaidOn = DateTime.Now;
            context.Orders.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.OrderNumber);
        }
        public async Task Update(UpdateOrder model, int corpClientId)
        {
            var entity = await context.Orders.FirstOrDefaultAsync(o => o.Customer.CorpClientId == corpClientId && o.OrderNumber == model.OrderNumber);

            if (entity.PaymentStatusId == PaymentStatusEnum.Pendente && model.PaymentStatusId == PaymentStatusEnum.Pago)
                entity.PaidOn = DateTime.Now;
            else if (entity.PaymentStatusId == PaymentStatusEnum.Pago && model.PaymentStatusId == PaymentStatusEnum.Pendente)
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

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            var data = await context.Orders.Where(o => o.OrderNumber == orderNumber
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
        public async Task<OrderStatusEnum> GetCurrentStatus(int orderNumber, int corpClientId)
        {
            return await context.Orders.Where(o => o.OrderNumber == orderNumber && o.Customer.CorpClientId == corpClientId).Select(o => o.OrderStatusId).FirstOrDefaultAsync();
        }
        public PagedResult<OrderSummary> GetOrderSummary(int corpClientId, string customer, DateTime? createdOnFrom, DateTime? createdOnTo,
                                                         DateTime? paidOnFrom, DateTime? paidOnTo,
                                                         DateTime? completeByFrom, DateTime? completeByTo,
                                                         int? paymentStatus, List<int> orderStatus, OrderSummaryOrderBy orderBy, OrderByDirection direction,
                                                         int? page, int? pageSize)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@corpClientId", corpClientId));
            var query = new StringBuilder(@"Select * FROM (SELECT C.CorpClientId,
	                        O.OrderId,
	                        O.OrderNumber,
	                        CASE C.CustomerTypeId WHEN 1 THEN RTRIM(c.Name + ' ' + ISNULL(c.LastName,'')) ELSE c.DBAName END AS Name,
	                        CASE C.CustomerTypeId WHEN 1 THEN c.CPF ELSE c.CNPJ END AS SocialIdentifier,
	                        P.PaymentStatusId  ,
	                        OS.OrderStatusId ,
                            C.CustomerTypeId,
	                        P.Description AS PaymentStatus,
	                        OS.Description AS OrderStatus,
	                        CAST( O.CompleteBy AS DATE) CompleteBy,
	                        CAST( O.CreatedOn AS DATE) CreatedOn,
	                        CAST( O.PaidOn AS DATE) PaidOn,
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
            if (paymentStatus.HasValue)
            {
                query.Append(" AND paymentStatusId = @paymentStatus");
                sqlParams.Add(new SqlParameter("@paymentStatus", paymentStatus) { SqlDbType = SqlDbType.Int });
            }
            if (orderStatus != null && orderStatus.Any())
                query.Append($" AND orderStatusId IN ({string.Join(',', orderStatus.ToArray())})");
            var count = context.OrderSummaries.FromSqlRaw(query.ToString(), sqlParams.ToArray()).Count();
            query.Append($" ORDER BY {orderBy} {direction}");

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
        public PagedResult<OrderItemReport> GetOrderItemReport(int corpClientId, int? orderNumber, OrderStatusEnum? orderStatus,
                                                               OrderItemStatusEnum? itemStatus, DateTime? completeByFrom, DateTime? completeByTo,
                                                               string customer, OrderItemReportOrderBy orderBy, OrderByDirection direction,
                                                               int? page, int? pageSize)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@corpClientId", corpClientId));
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
	                                        I.Quantity,
	                                        M.ShortName as MeasureUnit,
	                                        OIS.Description as OrderItemStatus,
	                                        OIS.OrderItemStatusId as OrderItemStatusId,
	                                        I.PriceAfterDiscount as FinalPrice,
	                                        I.LastStatusDate,
	                                        O.CompleteBy
                                        FROM [Order] O
                                        JOIN OrderStatus OS ON O.OrderStatusId = OS.OrderStatusId
                                        JOIN PaymentStatus PS ON O.PaymentStatusId = PS.PaymentStatusId
                                        JOIN OrderItem I ON O.OrderId = I.OrderId
                                        JOIN Customer C ON C.CustomerId = O.CustomerId
                                        JOIN OrderItemStatus OIS ON I.OrderItemStatusId = OIS.OrderItemStatusId
                                        JOIN MeasureUnit M ON I.MeasureUnitId = M.MeasureUnitId
                                        JOIN Product P ON I.ProductId = P.ProductId
                                        WHERE P.CorpClientId = @corpClientId AND C.CorpClientId = @corpClientId) X WHERE 1 = 1 ");

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
            if (itemStatus.HasValue)
            {
                query.Append($" AND OrderItemStatusId = @OrderItemStatusId");
                sqlParams.Add(new SqlParameter("@OrderItemStatusId", (int)itemStatus) { SqlDbType = SqlDbType.Int });
            }

            if (orderNumber.HasValue)
            {
                query.Append($" AND OrderNumber = @OrderNumber");
                sqlParams.Add(new SqlParameter("@OrderNumber", orderNumber.Value) { SqlDbType = SqlDbType.Int });
            }
            if (orderStatus.HasValue)
            {
                query.Append($" AND orderStatusId = @orderStatus");
                sqlParams.Add(new SqlParameter("@orderStatus", (int)orderStatus) { SqlDbType = SqlDbType.Int });
            }

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
    }
}
