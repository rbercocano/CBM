using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IOrderItemApp
    {
        Task<IEnumerable<OrderItem>> GetAll(int orderNumber, int corpClientId);
        Task Remove(long orderId, long orderItemId, int corpClientId);
        Task Update(UpdateOrderItem model, int corpClientId);
        Task<long> AddOrderItem(NewOrderItem model, int corpClientId);
        Task<int> GetLastItemNumber(int orderNumber, int corpClientId);
    }
}