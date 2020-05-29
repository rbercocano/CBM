using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class RawMaterialApp : IRawMaterialApp
    {
        private readonly IRawMaterialRepository rawMaterialRepository;

        public RawMaterialApp(IRawMaterialRepository rawMaterialRepository)
        {
            this.rawMaterialRepository = rawMaterialRepository;
        }

        public async Task<long> Add(NewRawMaterial model)
        {
            return await rawMaterialRepository.Add(model);
        }

        public async Task<RawMaterial> Get(int corpClientId, long id)
        {
            return await rawMaterialRepository.Get(corpClientId, id);
        }

        public async Task<IEnumerable<RawMaterial>> GetAll(int corpClientId, OrderByDirection direction)
        {
            return await rawMaterialRepository.GetAll(corpClientId, direction);
        }

        public async Task<PagedResult<RawMaterial>> GetPaged(int corpClientId, int page, int pageSize, string name, OrderByDirection direction)
        {
            return await rawMaterialRepository.GetPaged(corpClientId, page, pageSize, name, direction);
        }

        public async Task<RawMaterial> Update(UpdateRawmaterial model)
        {
            return await rawMaterialRepository.Update(model);
        }
    }
}
