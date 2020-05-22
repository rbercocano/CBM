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
            var lastNumber = context.Orders.Where(o => o.Customer.CorpClientId == corpClientId).Max(o => o.OrderNumber);
            model.OrderNumber = lastNumber + 1;
            var entity = mapper.Map<EF.Order>(model);
            context.Orders.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.OrderId);
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
    }
}
