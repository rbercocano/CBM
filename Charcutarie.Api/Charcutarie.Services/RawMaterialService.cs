using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class RawMaterialService : IRawMaterialService
    {
        private readonly IRawMaterialApp rawMaterialApp;

        public RawMaterialService(IRawMaterialApp rawMaterialApp)
        {
            this.rawMaterialApp = rawMaterialApp;
        }
        public async Task<PagedResult<RawMaterial>> GetPaged(int corpClientId, int page, int pageSize, string name, OrderByDirection direction)
        {
            return await rawMaterialApp.GetPaged(corpClientId, page, pageSize, name, direction);
        }
        public async Task<RawMaterial> Get(int corpClientId, int id)
        {
            return await rawMaterialApp.Get(corpClientId, id);
        }

        public async Task<IEnumerable<RawMaterial>> GetAll(int corpClientId, OrderByDirection direction)
        {
            return await rawMaterialApp.GetAll(corpClientId, direction);
        }

        public async Task<RawMaterial> Update(UpdateRawmaterial model)
        {
            return await rawMaterialApp.Update(model);
        }

        public async Task<long> Add(NewRawMaterial model)
        {
            return await rawMaterialApp.Add(model);
        }
    }
}
