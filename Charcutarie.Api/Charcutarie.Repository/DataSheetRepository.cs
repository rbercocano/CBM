using AutoMapper;
using Charcutarie.Models.ViewModels;
using ef = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class DataSheetRepository : IDataSheetRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public DataSheetRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<DataSheet> Get(long productId, int corpClientId)
        {
            var data = await context.DataSheets
                .Include(d => d.DataSheetItems)
                .ThenInclude(d => d.RawMaterial)
                .Where(d => d.ProductId == productId && d.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            var result = mapper.Map<DataSheet>(data);
            return result;
        }
        public async Task<DataSheet> Save(SaveDataSheet dataSheet, int corpClientId)
        {
            var data = await context.DataSheets
                .Include(d => d.DataSheetItems)
                .Where(d => d.ProductId == dataSheet.ProductId && d.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            if (data == null)
            {
                data = mapper.Map<ef.DataSheet>(dataSheet);
                context.DataSheets.Add(data);
                await context.SaveChangesAsync();
            }
            else
            {
                data.ProcedureDescription = dataSheet.ProcedureDescription;
                data.WeightVariationPercentage = dataSheet.WeightVariationPercentage;
                data.IncreaseWeight = dataSheet.IncreaseWeight;
                context.DataSheets.Update(data);
                await context.SaveChangesAsync();
            }

            return mapper.Map<DataSheet>(data); 
        }
    }
}
