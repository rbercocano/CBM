using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetPaged(int page, int pageSize, string filter, bool? active = null);
        Task<long> Add(NewUser model, int? corpClientId);
        Task<User> Update(UpdateUser model, int? corpClientId);
        Task<User> Get(int id);
        Task<IEnumerable<ParentModule>> GetUserModules(long userId);
    }
}
