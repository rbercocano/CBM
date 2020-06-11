using Charcutarie.Application.Contracts;
using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class OrderItemApp : IOrderItemApp
    {
        private readonly IOrderItemRepository repository;

        public OrderItemApp(IOrderItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<long> AddOrderItem(NewOrderItem model, int corpClientId)
        {
            return await repository.AddOrderItem(model, corpClientId);
        }

        public async Task<IEnumerable<OrderItem>> GetAll(int orderNumber, int corpClientId)
        {
            return await repository.GetAll(orderNumber, corpClientId);
        }

        public async Task Remove(long orderId, long orderItemId, int corpClientId)
        {
            await repository.Remove(orderItemId, corpClientId);
        }

        public async Task Update(UpdateOrderItem model, int corpClientId)
        {
            await repository.Update(model, corpClientId);
        }

        public async Task<int> GetLastItemNumber(int orderNumber, int corpClientId)
        {
            return await repository.GetLastItemNumber(orderNumber, corpClientId);
        }

        public async Task UpdateAllOrderItemStatus(int orderNumber, OrderItemStatusEnum status, int corpClientId)
        {
            await repository.UpdateAllOrderItemStatus(orderNumber, status, corpClientId);
        }

        public async Task<OrderItem> Get(long orderItemId, int corpClientId)
        {
            return await repository.Get(orderItemId, corpClientId);

        }
    }
}
