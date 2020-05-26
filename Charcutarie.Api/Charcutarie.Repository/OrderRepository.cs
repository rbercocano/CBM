using AutoMapper;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Data;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models;

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
            if (model.PaymentStatusId == 2)
                entity.PaidOn = DateTime.Now;
            context.Orders.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.OrderNumber);
        }
        public async Task Update(UpdateOrder model, int corpClientId)
        {
            var entity = await context.Orders.FirstOrDefaultAsync(o => o.Customer.CorpClientId == corpClientId && o.OrderNumber == model.OrderNumber);

            if (entity.PaymentStatusId == 1 && model.PaymentStatusId == 2)
                entity.PaidOn = DateTime.Now;
            else if (entity.PaymentStatusId == 2 && model.PaymentStatusId == 1)
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
            var cTypeId = context.Customers.Where(o => o.Orders.Any(p => p.OrderId == orderId) && o.CorpClientId == corpClientId).Select(c => c.CustomerTypeId).FirstOrDefault();
            if (cTypeId == 0)
                return null;
            if (cTypeId == 1)
            {
                var data = await context.PersonCustomers
                                    .Include(o => o.Contacts)
                                    .ThenInclude(o => o.ContactType)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.PaymentStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.Product)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.OrderItemStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.MeasureUnit)
                                    .FirstOrDefaultAsync(o => o.Orders.Any(p => p.OrderId == orderId) && o.CorpClientId == corpClientId);
                if (data == null) return null;
                var result = mapper.Map<Order>(data.Orders.FirstOrDefault());
                result.Customer = mapper.Map<MergedCustomer>(data);
                return result;
            }
            else
            {
                var data = await context.CompanyCustomers
                                    .Include(o => o.Contacts)
                                    .ThenInclude(o => o.ContactType)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.PaymentStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.Product)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.OrderItemStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.MeasureUnit)
                                    .FirstOrDefaultAsync(o => o.Orders.Any(p => p.OrderId == orderId) && o.CorpClientId == corpClientId);
                if (data == null) return null;
                var result = mapper.Map<Order>(data.Orders.FirstOrDefault());
                result.Customer = mapper.Map<MergedCustomer>(data);
                return result;
            }
        }

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            var cTypeId = context.Customers.Where(o => o.Orders.Any(p => p.OrderNumber == orderNumber) && o.CorpClientId == corpClientId).Select(c => c.CustomerTypeId).FirstOrDefault();
            if (cTypeId == 0)
                return null;
            if (cTypeId == 1)
            {
                var data = await context.PersonCustomers
                                    .Include(o => o.Contacts)
                                    .ThenInclude(o => o.ContactType)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.PaymentStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.Product)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.OrderItemStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.MeasureUnit)
                                    .FirstOrDefaultAsync(o => o.Orders.Any(p => p.OrderNumber == orderNumber) && o.CorpClientId == corpClientId);
                if (data == null) return null;
                var result = mapper.Map<Order>(data.Orders.FirstOrDefault());
                result.Customer = mapper.Map<MergedCustomer>(data);
                return result;
            }
            else
            {
                var data = await context.CompanyCustomers
                                    .Include(o => o.Contacts)
                                    .ThenInclude(o => o.ContactType)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.PaymentStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.Product)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.OrderItemStatus)
                                    .Include(o => o.Orders)
                                    .ThenInclude(o => o.OrderItems)
                                    .ThenInclude(o => o.MeasureUnit)
                                    .FirstOrDefaultAsync(o => o.Orders.Any(p => p.OrderNumber == orderNumber) && o.CorpClientId == corpClientId);
                if (data == null) return null;
                var result = mapper.Map<Order>(data.Orders.FirstOrDefault());
                result.Customer = mapper.Map<MergedCustomer>(data);
                return result;
            }
        }
        public async Task<int> GetCurrentStatus(int orderNumber, int corpClientId)
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
            {
                query.Append($" OFFSET {(page > 0 ? page - 1 : 0) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            }
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
    }
}
