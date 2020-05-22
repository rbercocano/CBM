using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IProductApp
    {
        Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string filter, bool? active = null);
        Task<long> Add(NewProduct model);

        Task<Product> Update(UpdateProduct model);
        Task<Product> Get(int corpClientId, long id);
        Task<IEnumerable<Product>> GetAll(int corpClientId);
        Task<IEnumerable<Product>> GetRange(int corpClientId, List<long> ids);
    }
}
