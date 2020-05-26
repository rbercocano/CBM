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
    public class PersonCustomerRepository : IPersonCustomerRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public PersonCustomerRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<long> Add(NewPersonCustomer model)
        {
            var entity = mapper.Map<EF.PersonCustomer>(model);
            context.Add(entity);
            var rows = await context.SaveChangesAsync();
            return await Task.FromResult(entity.CustomerId);
        }
        public async Task<PersonCustomer> Update(UpdatePersonCustomer model)
        {
            var entity = context.PersonCustomers.Where(p => p.CustomerId == model.CustomerId && p.CorpClientId == model.CorpClientId).FirstOrDefault();
            entity.Name = model.Name;
            entity.Cpf = model.Cpf;
            entity.LastName = model.LastName;
            entity.DateOfBirth = model.DateOfBirth;
            context.Update(entity);
            var rows = await context.SaveChangesAsync();
            var result = mapper.Map<PersonCustomer>(entity);
            return await Task.FromResult(result);
        }
        public async Task<PagedResult<PersonCustomer>> GetPaged(int page, int pageSize, string filter, int corpClientId)
        {
            var query = context.PersonCustomers.Include(p => p.CustomerType).Where(p => p.CorpClientId == corpClientId).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.Name.Contains(filter) || c.Cpf.Contains(filter));


            var count = query.Count();

            var data = await query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<PersonCustomer>
            {
                CurrentPage = page,
                RecordCount = count,
                Data = mapper.Map<IEnumerable<PersonCustomer>>(data),
                RecordsPerpage = pageSize
            };
        }
        public async Task<IEnumerable<PersonCustomer>> Filter(string filter, int corpClientId)
        {
            var query = context.PersonCustomers.Include(p => p.CustomerType).Where(c => c.CorpClientId == corpClientId && (c.Name.Contains(filter) || c.Cpf.Contains(filter)));
            var data = await query.ToListAsync();
            return mapper.Map<IEnumerable<PersonCustomer>>(data);
        }
        public async Task<PersonCustomer> Get(long id, int corpClientId)
        {
            var entity = await context.PersonCustomers.Include(p => p.CustomerType).Where(p => p.CorpClientId == corpClientId).FirstOrDefaultAsync(p => p.CustomerId == id);
            var result = mapper.Map<PersonCustomer>(entity);
            return result;
        }
    }
}
