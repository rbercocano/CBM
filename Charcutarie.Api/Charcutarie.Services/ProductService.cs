using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductApp productApp;

        public ProductService(IProductApp ProductApp)
        {
            this.productApp = ProductApp;
        }
        public async Task<PagedResult<Product>> GetPaged(int corpClientId, int page, int pageSize, string filter, bool? active = null)
        {
            return await productApp.GetPaged(corpClientId, page, pageSize, filter, active);
        }
        public async Task<long> Add(NewProduct model)
        {
            return await productApp.Add(model);
        }

        public async Task<Product> Update(UpdateProduct model)
        {
            return await productApp.Update(model);
        }
        public async Task<Product> Get(int corpClientId, int id)
        {
            return await productApp.Get(corpClientId, id);
        }

        public async Task<IEnumerable<Product>> GetAll(int corpClientId)
        {
            return await productApp.GetAll(corpClientId);
        }

        public async Task<IEnumerable<ProductionCostProfit>> GetProductionCostProfit(int corpClientId)
        {
            return await productApp.GetProductionCostProfit(corpClientId);
        }
    }
}
