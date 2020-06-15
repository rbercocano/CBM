using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IProductRepository
    {
        public Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string filter, bool? active = null);
        Task<long> Add(NewProduct model);
        Task<Product> Update(UpdateProduct model);
        Task<Product> Get(int corpClientId, long id);
        Task<IEnumerable<Product>> GetRange(int corpClientId, List<long> ids);
        Task<IEnumerable<Product>> GetAll(int corpClientId);
        Task<IEnumerable<ProductionCostProfit>> GetProductionCostProfit(int corpClientId);
        Task<IEnumerable<Production>> GetProduction(int corpClientId);
    }
}
