using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class SystemModuleApp : ISystemModuleApp
    {
        private readonly ISystemModuleRepository repository;

        public SystemModuleApp(ISystemModuleRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<ParentModule>> GetUserModules(long userId)
        {
            return await repository.GetUserModules(userId);
        }
    }
}
