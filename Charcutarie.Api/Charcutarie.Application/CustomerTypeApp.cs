using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class CustomerTypeApp : ICustomerTypeApp
    {
        private readonly ICustomerTypeRepository repository;

        public CustomerTypeApp(ICustomerTypeRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<CustomerType>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
