using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class SystemModuleRepository : ISystemModuleRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public SystemModuleRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        //public async Task<IEnumerable<SystemModule>> GetAll()
        //{
        //    var data = await context.SystemModules.ToListAsync();
        //    var result = mapper.Map<IEnumerable<SystemModule>>(data);
        //    return result;
        //}
        public async Task<IEnumerable<ParentModule>> GetUserModules(long userId)
        {
            var result = new List<ParentModule>();
            var data = await context.Users
                .Include(u => u.Role)
                .ThenInclude(u => u.RoleModules)
                .ThenInclude(u => u.SystemModule)
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Role.RoleModules.Select(rm => rm.SystemModule))
                .ToListAsync();
            var parentModules = data.OrderBy(o=>o.Order).Where(r => !r.ParentId.HasValue && r.Active);
            var childModules = data.OrderBy(o => o.Order).Where(r => r.ParentId.HasValue);
            foreach (var parent in parentModules)
            {
                result.Add(new ParentModule
                {
                    ModuleId = parent.SystemModuleId,
                    Name = parent.Name,
                    Route = parent.Route,
                    ChildModules = childModules.Where(c => c.ParentId == parent.SystemModuleId && c.Active).Select(c => new ChildModule
                    {
                        ModuleId = c.SystemModuleId,
                        Name = c.Name,
                        Route = c.Route
                    }).ToList()
                });
            }
            return result;
        }
    }
}
