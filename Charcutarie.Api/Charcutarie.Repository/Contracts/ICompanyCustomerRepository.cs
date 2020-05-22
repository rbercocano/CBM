using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ICompanyCustomerRepository
    {
        public Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter);
        Task<long> Add(NewCompanyCustomer model);
        Task<CompanyCustomer> Update(UpdateCompanyCustomer model);
        Task<CompanyCustomer> Get(long id);
        Task<IEnumerable<CompanyCustomer>> Filter(string filter);
    }
}
