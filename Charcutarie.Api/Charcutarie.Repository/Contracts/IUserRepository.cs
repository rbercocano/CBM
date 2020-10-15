using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IUserRepository
    {
        public Task<PagedResult<User>> GetPaged(int page, int pageSize, string filter, bool? active = null);
        Task<long> Add(NewUser model, int? corpClientId);
        Task<User> Update(UpdateUser model, int? corpClientId);
        Task<User> Get(long id, int corpClientId);
        Task<JWTUserInfo> DoLogin(string accountNumber, string username, string password);
        Task SaveRefreshToken(long userId, string refreshToken, DateTime createdOn);
        Task<string> GetRefreshToken(long userId);
        Task ChangePassword(long userId, string password, int corpClientId);
        Task<User> GetByLogin(string username, int corpClientId);
    }
}
