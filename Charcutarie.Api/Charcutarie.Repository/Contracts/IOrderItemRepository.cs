using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAll(long orderNumber, int corpClientId);
        Task Remove(long orderItemId, int corpClientId);
        Task Update(UpdateOrderItem model, int corpClientId);
        Task<long> AddOrderItem(NewOrderItem model, int corpClientId);
        Task<int> GetLastItemNumber(long orderNumber, int corpClientId);
        Task UpdateAllOrderItemStatus(long orderNumber, OrderItemStatusEnum status, int corpClientId);
        Task<OrderItem> Get(long orderItemId, int corpClientId);
    }
}