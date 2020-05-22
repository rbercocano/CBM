using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ICustomerContactRepository
    {
        Task<long> Add(AddCustomerContact model);
        Task Delete(long contactId, int corpClientId);
        Task<IEnumerable<CustomerContact>> GetAll(long customerId, int corpClientId);
        Task Update(UpdateCustomerContact model, int corpClientId);
    }
}