using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class ProductApp : IProductApp
    {
        private readonly IProductRepository productRepository;

        public ProductApp(IProductRepository ProductRepository)
        {
            this.productRepository = ProductRepository;
        }

        public async Task<long> Add(NewProduct model)
        {
            return await productRepository.Add(model);
        }

        public async Task<Product> Get(int corpClientId, long id)
        {            
            return await productRepository.Get(corpClientId, id);
        }
        public async Task<IEnumerable<Product>> GetRange(int corpClientId, List<long> ids)
        {
            return await productRepository.GetRange(corpClientId, ids);
        }

        public async Task<IEnumerable<Product>> GetAll(int corpClientId)
        {
            return await productRepository.GetAll(corpClientId);
        }

        public async Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string filter, bool? active = null)
        {
            return await productRepository.GetPaged(corpClientId, page, pageSize, filter, active);
        }

        public async Task<Product> Update(UpdateProduct model)
        {
            return await productRepository.Update(model);
        }

        public async Task<IEnumerable<ProductionCostProfit>> GetProductionCostProfit(int corpClientId)
        {
            return await productRepository.GetProductionCostProfit(corpClientId);
        }
    }
}
