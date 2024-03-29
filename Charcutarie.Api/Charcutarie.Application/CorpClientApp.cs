﻿using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class CorpClientApp : ICorpClientApp
    {
        private readonly ICorpClientRepository corpClientRepository;

        public CorpClientApp(ICorpClientRepository corpClientRepository)
        {
            this.corpClientRepository = corpClientRepository;
        }

        public async Task<CorpClient> Add(NewCorpClient model)
        {
            return await corpClientRepository.Add(model);
        }

        public async Task<CorpClient> Get(int id)
        {
            return await corpClientRepository.Get(id);
        }

        public async Task<IEnumerable<CorpClient>> GetActives()
        {
            return await corpClientRepository.GetActives();
        }

        public async Task<CorpClient> GetByCnpj(string cnpj)
        {
            return await corpClientRepository.GetByCnpj(cnpj);
        }

        public async Task<CorpClient> GetByCpf(string cpf)
        {
            return await corpClientRepository.GetByCpf(cpf);
        }

        public async Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            return await corpClientRepository.GetPaged(page, pageSize, filter, active);
        }

        public async Task<CorpClient> Update(UpdateCorpClient model)
        {
            return await corpClientRepository.Update(model);
        }
    }
}
