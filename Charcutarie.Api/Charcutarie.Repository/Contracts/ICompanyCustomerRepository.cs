using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ICompanyCustomerRepository
    {
        Task<long> Add(NewCompanyCustomer model);
        Task<CompanyCustomer> Update(UpdateCompanyCustomer model);
        Task<CompanyCustomer> Get(long id, int corpClientId);
        Task<IEnumerable<CompanyCustomer>> Filter(string filter, int corpClientId);
        public Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId);
    }
}
