using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string filter, bool? active = null);
        Task<long> Add(NewProduct model);
        Task<Product> Update(UpdateProduct model);
        Task<Product> Get(int corpClientId, int id);
        Task<IEnumerable<Product>> GetAll(int corpClientId);
    }
}
