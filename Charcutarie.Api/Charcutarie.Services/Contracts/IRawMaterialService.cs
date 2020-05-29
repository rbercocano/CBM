using Charcutarie.Models;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IRawMaterialService
    {
        Task<RawMaterial> Get(int corpClientId, int id);
        Task<IEnumerable<RawMaterial>> GetAll(int corpClientId, OrderByDirection direction);
        Task<PagedResult<RawMaterial>> GetPaged(int corpClientId, int page, int pageSize, string name, OrderByDirection direction);

        Task<RawMaterial> Update(UpdateRawmaterial model);
        Task<long> Add(NewRawMaterial model);
    }
}