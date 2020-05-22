using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IOrderService
    {
        Task<long> Add(NewOrder model, int corpClientId);
        Task<Order> Get(long orderId, int corpClientId);
        Task<Order> GetByNumber(int orderNumber, int corpClientId);
    }
}