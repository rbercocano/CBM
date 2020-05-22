using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class UserService : IUserService
    {
        private readonly IUserApp userApp;
        private readonly ISystemModuleApp systemModuleApp;

        public UserService(IUserApp userApp, ISystemModuleApp systemModuleApp)
        {
            this.userApp = userApp;
            this.systemModuleApp = systemModuleApp;
        }
        public async Task<PagedResult<User>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            return await userApp.GetPaged(page, pageSize, filter, active);
        }
        public async Task<long> Add(NewUser model, int? corpClientId)
        {
            return await userApp.Add(model, corpClientId);
        }

        public async Task<User> Update(UpdateUser model, int? corpClientId)
        {
            return await userApp.Update(model, corpClientId);
        }
        public async Task<User> Get(int id)
        {
            return await userApp.Get(id);
        }
        public async Task<IEnumerable<ParentModule>> GetUserModules(long userId)
        {
            return await systemModuleApp.GetUserModules(userId);
        }

    }
}
