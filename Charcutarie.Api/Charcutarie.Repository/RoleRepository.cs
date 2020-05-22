using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public RoleRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            var data = await context.Roles.ToListAsync();
            var result = mapper.Map<IEnumerable<Role>>(data);
            return result;
        }
    }
}
