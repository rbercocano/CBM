using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class CorpClientService : ICorpClientService
    {
        private readonly ICorpClientApp corpClientApp;
        private readonly IUserApp userApp;

        public CorpClientService(ICorpClientApp corpClientApp, IUserApp userApp)
        {
            this.corpClientApp = corpClientApp;
            this.userApp = userApp;
        }
        public async Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            return await corpClientApp.GetPaged(page, pageSize, filter, active);
        }
        public async Task<string> Register(ClientRegistration model)
        {
            var result = await corpClientApp.Add(model);
            return result.AccountNumber;
        }

        public async Task<CorpClient> Update(UpdateCorpClient model)
        {
            return await corpClientApp.Update(model);
        }
        public async Task<CorpClient> Get(int id)
        {
            return await corpClientApp.Get(id);
        }

        public async Task<IEnumerable<CorpClient>> GetActives()
        {
            return await corpClientApp.GetActives();
        }
    }
}
