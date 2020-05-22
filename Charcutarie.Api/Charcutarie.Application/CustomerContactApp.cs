using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class CustomerContactApp : ICustomerContactApp
    {
        private readonly ICustomerContactRepository repository;

        public CustomerContactApp(ICustomerContactRepository repository)
        {
            this.repository = repository;
        }

        public async Task<long> Add(AddCustomerContact model)
        {
            return await this.repository.Add(model);
        }

        public async Task Delete(long contactId, int corpClientId)
        {
            await this.repository.Delete(contactId, corpClientId);
        }

        public async Task<IEnumerable<CustomerContact>> GetAll(long customerId, int corpClientId)
        {
            return await this.repository.GetAll(customerId,corpClientId);
        }

        public async Task Update(UpdateCustomerContact model, int corpClientId)
        {
            await this.repository.Update(model, corpClientId);
        }
    }
}
