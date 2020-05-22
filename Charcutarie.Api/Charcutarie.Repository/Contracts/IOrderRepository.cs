using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<long> Add(NewOrder model, int corpClientId);
        Task<Order> Get(long orderId, int corpClientId);
        Task<Order> GetByNumber(int orderNumber, int corpClientId);
    }
}