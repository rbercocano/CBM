﻿using Charcutarie.Application.Contracts;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class CompanyCustomerApp : ICompanyCustomerApp
    {
        private readonly ICompanyCustomerRepository companyCustomerRepository;

        public CompanyCustomerApp(ICompanyCustomerRepository companyCustomerRepository)
        {
            this.companyCustomerRepository = companyCustomerRepository;
        }

        public async Task<long> Add(NewCompanyCustomer model)
        {
            return await companyCustomerRepository.Add(model);
        }

        public async Task<IEnumerable<CompanyCustomer>> Filter(string filter, int corpClientId)
        {
            return await companyCustomerRepository.Filter(filter, corpClientId);
        }

        public async Task<CompanyCustomer> Get(int id, int corpClientId)
        {
            return await companyCustomerRepository.Get(id, corpClientId);
        }

        public async Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId)
        {
            return await companyCustomerRepository.GetPaged(page, pageSize, filter, corpClientId);
        }

        public async Task<CompanyCustomer> Update(UpdateCompanyCustomer model)
        {
            return await companyCustomerRepository.Update(model);
        }
    }
}
