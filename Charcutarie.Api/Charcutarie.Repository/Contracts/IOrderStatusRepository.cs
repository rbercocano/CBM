using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IOrderStatusRepository
    {
        public Task<IEnumerable<OrderStatus>> GetAll();
    }
}
