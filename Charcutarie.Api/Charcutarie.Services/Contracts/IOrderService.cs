using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IOrderService
    {
        Task<int> Add(NewOrder model, int corpClientId);
        Task<Order> Get(long orderId, int corpClientId);
        Task<Order> GetByNumber(int orderNumber, int corpClientId);
        Task Update(UpdateOrder model, int corpClientId);
        Task ChangeStatus(UpdateOrderStatus model, int corpClientId);
        Task<int> RestoreOrderStatus(int orderNumber, int corpClientId);
        Task RemoveOrderItem(long orderId, long orderItemId, int corpClientId);
        Task UpdateOrderItem(UpdateOrderItem model, int corpClientId);
        Task<long> AddOrderItem(NewOrderItem model, int corpClientId);
    }
}