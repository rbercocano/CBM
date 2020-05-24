using AutoMapper;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public OrderItemRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<OrderItem>> GetAll(int orderNumber, int corpClientId)
        {
            var data = await context.OrderItems
                .Where(i => i.Order.OrderNumber == orderNumber && i.Order.Customer.CorpClientId == corpClientId)
                .ToListAsync();
            var result = mapper.Map<IEnumerable<OrderItem>>(data);
            return result;
        }
        public async Task<long> AddOrderItem(NewOrderItem model, int corpClientId)
        {
            var orderItem = mapper.Map<EF.OrderItem>(model);
            context.OrderItems.Add(orderItem);
            var result = await context.SaveChangesAsync();
            return orderItem.OrderItemId;
        }
        public async Task Remove(long orderItemId, int corpClientId)
        {
            var data = await context.OrderItems
                  .Where(i => i.OrderItemId == orderItemId && i.Order.Customer.CorpClientId == corpClientId)
                  .FirstOrDefaultAsync();
            context.OrderItems.Remove(data);
            await context.SaveChangesAsync();
        }
        public async Task Update(UpdateOrderItem model, int corpClientId)
        {
            var data = await context.OrderItems
                  .Where(i => i.OrderItemId == model.OrderItemId && i.Order.Customer.CorpClientId == corpClientId)
                  .FirstOrDefaultAsync();
            data.MeasureUnitId = model.MeasureUnitId;
            data.Quantity = model.Quantity;
            data.Discount = model.Discount;
            data.AdditionalInfo = model.AdditionalInfo;
            data.OrderItemStatusId = model.OrderItemStatusId;
            data.OriginalPrice = model.OriginalPrice;
            data.PriceAfterDiscount = model.PriceAfterDiscount;
            context.OrderItems.Update(data);
            await context.SaveChangesAsync();
        }
        public async Task<int> GetLastItemNumber(int orderNumber, int corpClientId)
        {
            var query = context.OrderItems
                .Where(i => i.Order.OrderNumber == orderNumber && i.Order.Customer.CorpClientId == corpClientId);
            var total = await query.CountAsync();
            if (total == 0)
                return 0;
            var data = await query.MaxAsync(i => i.ItemNumber);
            return data;
        }
    }
}
