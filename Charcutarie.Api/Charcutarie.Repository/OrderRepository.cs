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
        public async Task GetOrderSummary()
        {
            var query = @"SELECT 
	                        O.OrderId,
	                        O.OrderNumber,
	                        CASE C.CustomerTypeId WHEN 1 THEN RTRIM(c.Name + ' ' + ISNULL(c.LastName,'')) ELSE c.DBAName END AS Name,
	                        CASE C.CustomerTypeId WHEN 1 THEN c.CPF ELSE c.CNPJ END AS SocialIdentifier,
	                        P.Description AS PaymentStatus,
	                        OS.Description AS OrderStatus,
	                        O.CompleteBy,
	                        O.CreatedOn,
	                        O.PaidOn,
	                        (SELECT SUM(OI.PriceAfterDiscount) FROM OrderItem OI WHERE OI.OrderId = O.OrderId AND OI.OrderItemStatusId != 4) AS FinalPrice
                        FROM Customer C 
                        JOIN [Order] O ON C.CustomerId = O.CustomerId
                        JOIN PaymentStatus P ON O.PaymentStatusId = P.PaymentStatusId
                        JOIN OrderStatus OS ON O.OrderStatusId = OS.OrderStatusId";
        }
    }
}
