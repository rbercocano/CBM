using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class MeasureUnitRepositoy : IMeasureUnitRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public MeasureUnitRepositoy(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MeasureUnit>> GetAll()
        {
            var data = await context.MeasureUnits.ToListAsync();
            var result = mapper.Map<IEnumerable<MeasureUnit>>(data);
            return result;
        }
    }
}
