﻿using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class UserApp : IUserApp
    {
        private readonly IUserRepository userRepository;

        public UserApp(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<JWTUserInfo> DoLogin(int corpClientId, string username, string password)
        {
            return await userRepository.DoLogin(corpClientId, username, password);
        }

        public async Task<User> Get(int id)
        {
            return await userRepository.Get(id);
        }

        public async Task<PagedResult<User>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            return await userRepository.GetPaged(page, pageSize, filter, active);
        }

        public async Task<long> Add(NewUser model, int? corpClientId)
        {
            return await userRepository.Add(model, corpClientId);
        }
        public async Task<User> Update(UpdateUser model, int? corpClientId)
        {
            return await userRepository.Update(model, corpClientId);
        }
        public async Task SaveRefreshToken(long userId, string refreshToken, DateTime createdOn)
        {
            await userRepository.SaveRefreshToken(userId, refreshToken, createdOn);
        }

        public async Task<string> GetRefreshToken(long userId)
        {
            return await userRepository.GetRefreshToken(userId);
        }
    }
}
