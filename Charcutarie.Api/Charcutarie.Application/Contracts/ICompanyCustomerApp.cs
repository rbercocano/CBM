using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ICompanyCustomerApp
    {
        Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter);
        Task<long> Add(NewCompanyCustomer model);
        Task<CompanyCustomer> Update(UpdateCompanyCustomer model);
        Task<CompanyCustomer> Get(int id);
        Task<IEnumerable<CompanyCustomer>> Filter(string filter);
    }
}
