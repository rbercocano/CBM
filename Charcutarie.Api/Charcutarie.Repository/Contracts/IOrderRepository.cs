using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<int> Add(NewOrder model, int corpClientId);
        Task<Order> Get(long orderId, int corpClientId);
        Task<Order> GetByNumber(int orderNumber, int corpClientId);
        Task Update(UpdateOrder model, int corpClientId);
        Task ChangeStatus(UpdateOrderStatus model, int corpClientId);
        Task<int> GetCurrentStatus(int orderNumber, int corpClientId);
    }
}