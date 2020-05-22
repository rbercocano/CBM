using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class OrderItemStatusApp : IOrderItemStatusApp
    {
        private readonly IOrderItemStatusRepository repository;

        public OrderItemStatusApp(IOrderItemStatusRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<OrderItemStatus>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
