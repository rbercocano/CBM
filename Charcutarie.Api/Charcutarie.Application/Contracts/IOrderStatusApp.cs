
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IOrderStatusApp
    {
        Task<IEnumerable<OrderStatus>> GetAll();
    }
}
