using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class OrderStatusApp : IOrderStatusApp
    {
        private readonly IOrderStatusRepository repository;

        public OrderStatusApp(IOrderStatusRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<OrderStatus>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
