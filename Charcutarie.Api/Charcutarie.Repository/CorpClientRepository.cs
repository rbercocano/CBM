using AutoMapper;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using EF = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class CorpClientRepository : ICorpClientRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public CorpClientRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<int> Add(NewCorpClient model)
        {
            var entity = mapper.Map<EF.CorpClient>(model);
            context.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.CorpClientId);
        }
        public async Task<CorpClient> Update(UpdateCorpClient model)
        {
            var entity = context.CorpClients.Find(model.CorpClientId);
            entity.DBAName = model.DBAName;
            entity.Name = model.Name;
            entity.Active = model.Active;
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            return mapper.Map<CorpClient>(entity);
        }
        public async Task<PagedResult<CorpClient>> GetPaged(int page, int pageSize, string filter, bool? active = null)
        {
            var query = context.CorpClients.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.Name.Contains(filter) || c.DBAName.Contains(filter));

            if (active.HasValue)
                query = query.Where(c => c.Active == active);

            var count = query.Count();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<CorpClient>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<CorpClient>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<CorpClient> Get(int id)
        {
            var entity = context.CorpClients.Find(id);
            var result = mapper.Map<CorpClient>(entity);
            return await Task.FromResult(result);
        }
        public async Task<IEnumerable<CorpClient>> GetActives()
        {
            var data = await context.CorpClients.Where(c => c.Active).ToListAsync();
            return mapper.Map<IEnumerable<CorpClient>>(data);
        }
    }
}
