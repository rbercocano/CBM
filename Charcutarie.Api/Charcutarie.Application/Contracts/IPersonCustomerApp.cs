using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IPersonCustomerApp
    {
        Task<PagedResult<PersonCustomer>> GetPaged(int page, int pageSize, string filter);
        Task<long> Add(NewPersonCustomer model);

        Task<PersonCustomer> Update(UpdatePersonCustomer model);
        Task<PersonCustomer> Get(int id);
        Task<IEnumerable<PersonCustomer>> Filter(string filter);
    }
}
