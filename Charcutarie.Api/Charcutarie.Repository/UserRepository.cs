using AutoMapper;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Charcutarie.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public UserRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<long> Add(NewUser model, int? corpClientId)
        {
            var entity = mapper.Map<EF.User>(model);
            entity.CorpClientId = corpClientId;
            context.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.UserId);
        }
        public async Task<User> Update(UpdateUser model, int? corpClientId)
        {
            var entity = context.Users.Find(model.UserId);
            entity.Name = model.Name;
            entity.Active = model.Active;
            entity.Username = model.Username;
            entity.CorpClientId = corpClientId;
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            var result = mapper.Map<User>(entity);
            return await Task.FromResult(result);
        }
        public async Task<PagedResult<User>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            var query = context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.Name.Contains(filter) || c.Username.Contains(filter));

            if (active.HasValue)
                query = query.Where(c => c.Active == active);

            var count = query.Count();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<User>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<User>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<User> Get(long id, int corpClientId)
        {
            var entity = await context.Users.FirstOrDefaultAsync(p => p.UserId == id && p.CorpClientId == corpClientId);
            var result = mapper.Map<User>(entity);
            return result;
        }
        public async Task<User> GetByLogin(string username, int corpClientId)
        {
            var entity = await context.Users
                .FirstOrDefaultAsync(p => p.Username == username
                                                                  && p.CorpClientId == corpClientId
                                                                  && p.Active
                                                                  && p.CorpClient.Active);
            var result = mapper.Map<User>(entity);
            return result;
        }

        public async Task<JWTUserInfo> DoLogin(int corpClientId, string username, string password)
        {
            var entity = await context.Users
                .Include(c => c.CorpClient)
                .Include(c => c.Role)
                .FirstOrDefaultAsync(p => p.Username == username
                                                                  && p.Password == password
                                                                  && p.CorpClientId == corpClientId
                                                                  && p.Active
                                                                  && p.CorpClient.Active);
            var result = mapper.Map<JWTUserInfo>(entity);
            return result;
        }
        public async Task SaveRefreshToken(long userId, string refreshToken, DateTime createdOn)
        {
            var existingToken = await context.UserTokens.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existingToken == null)
            {
                existingToken = new EF.UserToken { UserId = userId, Token = refreshToken, CreatedOn = createdOn };
                context.UserTokens.Add(existingToken);
                await context.SaveChangesAsync();
                return;
            }
            existingToken.Token = refreshToken;
            existingToken.CreatedOn = createdOn;
            context.UserTokens.Update(existingToken);
            await context.SaveChangesAsync();

        }
        public async Task<string> GetRefreshToken(long userId)
        {
            var existingToken = await context.UserTokens.FirstOrDefaultAsync(u => u.UserId == userId);
            return existingToken?.Token;
        }
        public async Task ChangePassword(long userId, string password, int corpClientId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId && u.CorpClientId == corpClientId);
            user.Password = password;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
