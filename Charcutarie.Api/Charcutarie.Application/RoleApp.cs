using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class RoleApp : IRoleApp
    {
        private readonly IRoleRepository repository;

        public RoleApp(IRoleRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
