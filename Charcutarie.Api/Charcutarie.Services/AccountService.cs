using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserApp userApp;

        public AccountService(IUserApp userApp)
        {
            this.userApp = userApp;
        }
        public async Task<JWTUserInfo> DoLogin(int corpClientId, string username, string password)
        {
            return await userApp.DoLogin(corpClientId, username, password);
        }

        public async Task<string> GetRefreshToken(long userId)
        {
            return await userApp.GetRefreshToken(userId);
        }

        public async Task SaveRefreshToken(long userId, string refreshToken, DateTime createdOn)
        {
            await userApp.SaveRefreshToken(userId, refreshToken, createdOn);
        }
    }
}
