using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface ICustomerService
    {
        Task<long> Add(Customer model);
        Task<Customer> Get(int id, int customerTypeId);
        Task<PagedResult<T>> GetPaged<T>(int page, int pageSize, string filter, int customerTypeId) where T : Customer;
        Task<Customer> Update(Customer model);


        Task<long> AddContact(AddCustomerContact model);
        Task DeleteContact(long contactId, int corpClientId);
        Task<IEnumerable<CustomerContact>> GetAllContacts(long customerId, int corpClientId);
        Task UpdateContact(UpdateCustomerContact model, int corpClientId);

        Task<IEnumerable<MergedCustomer>> FilterCustomers(string filter);
    }
}