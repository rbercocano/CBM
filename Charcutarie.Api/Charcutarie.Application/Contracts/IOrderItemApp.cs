using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IOrderItemApp
    {
        Task<IEnumerable<OrderItem>> GetAll(long orderNumber, int corpClientId);
        Task Remove(long orderId, long orderItemId, int corpClientId);
        Task Update(UpdateOrderItem model, int corpClientId);
        Task<long> AddOrderItem(NewOrderItem model, int corpClientId);
        Task<int> GetLastItemNumber(long orderNumber, int corpClientId);
        Task UpdateAllOrderItemStatus(long orderNumber, OrderItemStatusEnum status, int corpClientId);
        Task<OrderItem> Get(long orderItemId, int corpClientId);
    }
}