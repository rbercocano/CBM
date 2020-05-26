using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ICompanyCustomerApp
    {
        Task<long> Add(NewCompanyCustomer model);
        Task<CompanyCustomer> Update(UpdateCompanyCustomer model);
        Task<CompanyCustomer> Get(int id, int corpClientId);
        Task<IEnumerable<CompanyCustomer>> Filter(string filter, int corpClientId);
        Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId);
    }
}
