using Charcutarie.Models;
using Charcutarie.Models.Enums.OrderBy;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IRawMaterialRepository
    {
        Task<RawMaterial> Get(int corpClientId, long id);
        Task<IEnumerable<RawMaterial>> GetAll(int corpClientId, OrderByDirection direction);
        Task<PagedResult<RawMaterial>> GetPaged(int corpClientId, int page, int pageSize, string name, OrderByDirection direction);
    }
}