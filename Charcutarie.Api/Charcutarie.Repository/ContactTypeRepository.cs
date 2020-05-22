using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class ContactTypeRepository : IContactTypeRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public ContactTypeRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ContactType>> GetAll()
        {
            var data = await context.ContactTypes.ToListAsync();
            var result = mapper.Map<IEnumerable<ContactType>>(data);
            return result;
        }
    }
}
