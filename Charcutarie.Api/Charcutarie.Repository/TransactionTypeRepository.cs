using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public TransactionTypeRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<TransactionType>> GetAll()
        {
            var data = await context.TransactionTypes.ToListAsync();
            var result = mapper.Map<IEnumerable<TransactionType>>(data);
            return result;
        }
        public async Task<TransactionType> Get(int id)
        {
            var data = await context.TransactionTypes.FirstOrDefaultAsync(t=>t.TransactionTypeId == id);
            var result = mapper.Map<TransactionType>(data);
            return result;
        }
    }
}
