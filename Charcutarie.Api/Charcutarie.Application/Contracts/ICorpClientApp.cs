using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ICorpClientApp
    {
        Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null);
        Task<CorpClient> Add(ClientRegistration model);

        Task<CorpClient> Update(UpdateCorpClient model);
        Task<CorpClient> Get(int id);
        Task<IEnumerable<CorpClient>> GetActives();
    }
}
