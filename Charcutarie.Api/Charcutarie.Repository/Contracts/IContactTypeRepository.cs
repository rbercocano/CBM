using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IContactTypeRepository
    {
        public Task<IEnumerable<ContactType>> GetAll();
    }
}
