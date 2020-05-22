using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class OrderApp : IOrderApp
    {
        private readonly IOrderRepository repository;

        public OrderApp(IOrderRepository repository)
        {
            this.repository = repository;
        }
        public async Task<long> Add(NewOrder model, int corpClientId)
        {
            return await repository.Add(model, corpClientId);
        }

        public async Task<Order> Get(long orderId, int corpClientId)
        {
            return await repository.Get(orderId, corpClientId);
        }

        public async Task<Order> GetByNumber(int orderNumber, int corpClientId)
        {
            return await repository.GetByNumber(orderNumber, corpClientId);
        }
    }
}
