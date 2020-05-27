using AutoMapper;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charcutarie.Models.Enums.OrderBy;

namespace Charcutarie.Repository
{
    public class RawMaterialRepository : IRawMaterialRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public RawMaterialRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<PagedResult<RawMaterial>> GetPaged(int corpClientId, int page, int pageSize, string name, OrderByDirection direction)
        {
            var query = context.RawMaterials.Include(p => p.MeasureUnit).Where(p => p.CorpClientId == corpClientId).AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            query = direction == OrderByDirection.Asc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
            var count = query.Count();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<RawMaterial>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<RawMaterial>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<RawMaterial> Get(int corpClientId, long id)
        {
            var entity = await context.RawMaterials.Include(p => p.MeasureUnit).FirstOrDefaultAsync(p => p.RawMaterialId == id && p.CorpClientId == corpClientId);
            var result = mapper.Map<RawMaterial>(entity);
            return result;
        }
        public async Task<IEnumerable<RawMaterial>> GetAll(int corpClientId, OrderByDirection direction)
        {
            var query = context.RawMaterials.Include(p => p.MeasureUnit).Where(p => p.CorpClientId == corpClientId);
            query = direction == OrderByDirection.Asc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
            var entity = await query.ToListAsync();
            var result = mapper.Map<IEnumerable<RawMaterial>>(entity);
            return result;
        }
    }
}
