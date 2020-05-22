using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class ContactTypeApp : IContactTypeApp
    {
        private readonly IContactTypeRepository repository;

        public ContactTypeApp(IContactTypeRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<ContactType>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
