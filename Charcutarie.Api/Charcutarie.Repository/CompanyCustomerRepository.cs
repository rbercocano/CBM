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
    public class CompanyCustomerRepository : ICompanyCustomerRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public CompanyCustomerRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<long> Add(NewCompanyCustomer model)
        {
            var entity = mapper.Map<EF.CompanyCustomer>(model);
            context.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.CustomerId);
        }
        public async Task<CompanyCustomer> Update(UpdateCompanyCustomer model)
        {
            var entity = context.CompanyCustomers.Where(p => p.CustomerId == model.CustomerId && p.CorpClientId == model.CorpClientId).FirstOrDefault();
            entity.Name = model.Name;
            entity.Cnpj = model.Cnpj;
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            var result = mapper.Map<CompanyCustomer>(entity);
            return await Task.FromResult(result);
        }
        public async Task<PagedResult<CompanyCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId)
        {
            var query = context.CompanyCustomers.Include(p => p.CustomerType).Where(c => c.CorpClientId == corpClientId).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.DBAName.Contains(filter) || c.Name.Contains(filter) || c.Cnpj.Contains(filter));


            var count = query.Count();

            var data = await query.OrderBy(c => c.DBAName).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<CompanyCustomer>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<CompanyCustomer>>(data),
                RecordsPerpage = pageSize
            };
        }

        public async Task<IEnumerable<CompanyCustomer>> Filter(string filter, int corpClientId)
        {
            var query = context.CompanyCustomers.Include(p => p.CustomerType).Where(c => c.CorpClientId == corpClientId).AsQueryable();
            query = query = query.Where(c => c.DBAName.Contains(filter) || c.Name.Contains(filter) || c.Cnpj.Contains(filter));
            var data = await query.ToListAsync();
            return mapper.Map<IEnumerable<CompanyCustomer>>(data);
        }
        public async Task<CompanyCustomer> Get(long id, int corpClientId)
        {
            var entity = await context.CompanyCustomers.Include(p => p.CustomerType).FirstOrDefaultAsync(p => p.CustomerId == id && p.CorpClientId == corpClientId);
            var result = mapper.Map<CompanyCustomer>(entity);
            return result;
        }
    }
}
