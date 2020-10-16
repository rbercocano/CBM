using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ICorpClientRepository
    {
        public Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null);
        Task<CorpClient> Add(NewCorpClient model);
        Task<CorpClient> Update(UpdateCorpClient model);
        Task<CorpClient> Get(int id);
        Task<IEnumerable<CorpClient>> GetActives();
        Task<CorpClient> GetByCnpj(string cnpj);
        Task<CorpClient> GetByCpf(string cpf);
    }
}
