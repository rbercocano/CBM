using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public CustomerTypeRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<CustomerType>> GetAll()
        {
            var data = await context.CustomerTypes.ToListAsync();
            var result = mapper.Map<IEnumerable<CustomerType>>(data);
            return result;
        }
    }
}
