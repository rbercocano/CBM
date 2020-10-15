using Charcutarie.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IAccountService
    {
        Task<JWTUserInfo> DoLogin(string accountNumber, string username, string password);
        Task SaveRefreshToken(long userId, string refreshToken, DateTime createdOn);
        Task<string> GetRefreshToken(long userId);
    }
}