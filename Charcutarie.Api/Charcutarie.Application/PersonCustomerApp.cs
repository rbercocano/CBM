﻿using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class PersonCustomerApp : IPersonCustomerApp
    {
        private readonly IPersonCustomerRepository personCustomerRepository;

        public PersonCustomerApp(IPersonCustomerRepository personCustomerRepository)
        {
            this.personCustomerRepository = personCustomerRepository;
        }

        public async Task<long> Add(NewPersonCustomer model)
        {
            return await personCustomerRepository.Add(model);
        }

        public async Task<IEnumerable<PersonCustomer>> Filter(string filter, int corpClientId)
        {
            return await personCustomerRepository.Filter(filter, corpClientId);
        }

        public async Task<PersonCustomer> Get(int id, int corpClientId)
        {
            return await personCustomerRepository.Get(id, corpClientId);
        }

        public async Task<PagedResult<PersonCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId)
        {
            return await personCustomerRepository.GetPaged(page, pageSize, filter, corpClientId);
        }

        public async Task<PersonCustomer> Update(UpdatePersonCustomer model)
        {
            return await personCustomerRepository.Update(model);
        }
    }
}
