using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class SystemModuleService : ISystemModuleService
    {
        private readonly ISystemModuleApp systemModuleApp;

        public SystemModuleService(ISystemModuleApp SystemModuleApp)
        {
            this.systemModuleApp = SystemModuleApp;
        }
        public async Task<IEnumerable<ParentModule>> GetUserModules(long userId)
        {
            return await systemModuleApp.GetUserModules(userId);
        }
    }
}
