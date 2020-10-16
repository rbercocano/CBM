using Charcutarie.Application.Contracts;
using Charcutarie.Core.ExceptionHandling;
using Charcutarie.Core.SMTP;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Charcutarie.Services
{
    public class CorpClientService : ICorpClientService
    {
        private readonly ICorpClientApp corpClientApp;
        private readonly IUserApp userApp;
        private readonly IEmailManager emailManager;

        public CorpClientService(ICorpClientApp corpClientApp, IUserApp userApp, IEmailManager emailManager)
        {
            this.corpClientApp = corpClientApp;
            this.userApp = userApp;
            this.emailManager = emailManager;
        }
        public async Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            return await corpClientApp.GetPaged(page, pageSize, filter, active);
        }
        public async Task<CorpClient> Register(ClientRegistration model)
        {
            model.Mobile = $"+55 {model.Mobile}";
            var type = model.CustomerTypeId == 1 ? "CPF" : "CNPJ";
            var existingClient = await (model.CustomerTypeId == 1 ?
                    corpClientApp.GetByCpf(model.SocialIdentifier) :
                    corpClientApp.GetByCnpj(model.SocialIdentifier));
            if (existingClient != null)
                throw new BusinessException($"{type} já cadastrado no sistema.");

            var result = await corpClientApp.Add(new NewCorpClient
            {
                CNPJ = model.CustomerTypeId == 1 ? string.Empty : model.SocialIdentifier,
                CPF = model.CustomerTypeId == 2 ? string.Empty : model.SocialIdentifier,
                Currency = "R$",
                CustomerTypeId = model.CustomerTypeId,
                DBAName = model.CustomerTypeId == 1 ? model.Name : model.DbaName,
                Name = model.Name,
                LicenseExpirationDate = null,
                Active = true,
                Email = model.Email,
                Mobile = model.Mobile
            });
            var user = await userApp.Add(new NewUser
            {
                Active = true,
                DefaultMassMid = 2,
                DefaultVolumeMid = 6,
                Email = model.Email,
                Mobile = model.Mobile,
                Name = model.Name,
                Password = model.Password,
                RoleId = 2,
                Username = model.Username
            }, result.CorpClientId);
            emailManager.SendRegistrationEmail(result.AccountNumber, model.Username, model.Email, model.Name, model.CustomerTypeId, model.SocialIdentifier);
            return result;
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
