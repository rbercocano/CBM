using Charcutarie.Application.Contracts;
using Charcutarie.Core.ExceptionHandling;
using Charcutarie.Core.SMTP;
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
        private readonly IEmailManager emailManager;

        public UserService(IUserApp userApp, ISystemModuleApp systemModuleApp, IEmailManager emailManager)
        {
            this.userApp = userApp;
            this.systemModuleApp = systemModuleApp;
            this.emailManager = emailManager;
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
        public async Task<User> Get(long id, int corpClientId)
        {
            return await userApp.Get(id, corpClientId);
        }
        public async Task<IEnumerable<ParentModule>> GetUserModules(long userId)
        {
            return await systemModuleApp.GetUserModules(userId);
        }

        public async Task<string> ResetPassword(string userName, int corpClientId)
        {
            var user = await userApp.GetByLogin(userName, corpClientId);
            if (user == null)
                throw new BusinessException("Usuário não encontrado");
            var newPassword = await userApp.ResetPassword(user.UserId, corpClientId);
            var body = $"<p>Sua senha foi redefinida conforme solicitado.</p><p>Nova Senha: <b>{newPassword}</b></p>";
            var subject = "Charcuterie Business Manager - Nova Senha";
            emailManager.SendEmail(new List<string>() { user.Email }, body, subject, true);
            return user.Email;
        }
        public async Task ChangePassword(ChangePassword model, int corpClientId, long userId)
        {
            await userApp.ChangePassword(model, corpClientId, userId);
        }
    }
}
