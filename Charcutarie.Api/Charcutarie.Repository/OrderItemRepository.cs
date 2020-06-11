using AutoMapper;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Charcutarie.Models.Enums;
using Microsoft.Data.SqlClient;

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
        public async Task<OrderItem> Get(long orderItemId, int corpClientId)
        {
            var data = await context.OrderItems
                .Where(i => i.OrderItemId == orderItemId && i.Order.Customer.CorpClientId == corpClientId)
                .Include(o => o.Order)
                .FirstOrDefaultAsync();
            var result = mapper.Map<OrderItem>(data);
            return result;
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
            orderItem.LastStatusDate = DateTime.Now;
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
            if (model.OrderItemStatusId != data.OrderItemStatusId)
                data.LastStatusDate = DateTime.Now;
            data.MeasureUnitId = model.MeasureUnitId;
            data.Quantity = model.Quantity;
            data.Discount = model.Discount;
            data.AdditionalInfo = model.AdditionalInfo;
            data.OrderItemStatusId = model.OrderItemStatusId;
            data.OriginalPrice = model.OriginalPrice;
            data.PriceAfterDiscount = model.PriceAfterDiscount;
            data.Cost = model.Cost;
            data.Profit = model.Profit;
            data.ProfitPercentage = model.ProfitPercentage;
            context.OrderItems.Update(data);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAllOrderItemStatus(int orderNumber, OrderItemStatusEnum status, int corpClientId)
        {
            var order = await context.OrderItems
                     .Where(i => i.Order.OrderNumber == orderNumber && i.Order.Customer.CorpClientId == corpClientId).FirstOrDefaultAsync();
            if (order == null) return;
            var query = @"UPDATE OrderItem 
                        SET OrderItemStatusId = @status,
                        LastUpdated = GETDATE(),
                        LastStatusDate = CASE WHEN OrderItemStatusId = @status THEN LastStatusDate ELSE GETDATE() END
                        WHERE OrderId = @order AND OrderItemStatusId != 3";
            var sqlParams = new[]
            {
                new SqlParameter("@status", (int)status),
                new SqlParameter("@order", order.OrderId)
            };
            context.Database.ExecuteSqlCommand(query, sqlParams);
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
