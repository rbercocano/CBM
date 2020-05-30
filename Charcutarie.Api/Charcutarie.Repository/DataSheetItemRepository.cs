using AutoMapper;
using Charcutarie.Models.ViewModels;
using ef = Charcutarie.Models.Entities;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Repository
{
    public class DataSheetItemRepository : IDataSheetItemRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public DataSheetItemRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<DataSheetItem> Get(long itemId, int corpClientId)
        {
            var data = await context.DataSheetItems
                .Where(d => d.DataSheetItemId == itemId && d.DataSheet.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            var result = mapper.Map<DataSheetItem>(data);
            return result;
        }
        public async Task<long> Add(NewDataSheetItem item, int corpClientId)
        {
            var data = await context.DataSheets
                .Where(d => d.ProductId == item.ProductId && d.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            var entity = mapper.Map<ef.DataSheetItem>(data);
            entity.DataSheetId = data.DataSheetId;
            context.DataSheetItems.Add(entity);
            await context.SaveChangesAsync();
            return entity.DataSheetItemId;
        }
        public async Task<IEnumerable<DataSheetItem>> AddRange(IEnumerable<NewDataSheetItem> items, int corpClientId)
        {
            var data = await context.DataSheets
                .Where(d => d.ProductId == items.FirstOrDefault().ProductId && d.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            var entities = mapper.Map<IEnumerable<ef.DataSheetItem>>(data);
            entities.ToList().ForEach(e => e.DataSheetId = data.DataSheetId);
            context.DataSheetItems.AddRange(entities);
            await context.SaveChangesAsync();
            return mapper.Map<IEnumerable<DataSheetItem>>(entities);
        }

        public async Task Update(UpdateDataSheetItem item, int corpClientId)
        {
            var data = await context.DataSheetItems
                .Where(d => d.DataSheetItemId == item.DataSheetItemId && d.DataSheet.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            data.Percentage = item.Percentage;
            data.AdditionalInfo = item.AdditionalInfo;
            context.DataSheetItems.Update(data);
            await context.SaveChangesAsync(); ;
        }
        public async Task Delete(long itemId, int corpClientId)
        {
            var data = await context.DataSheetItems
                .Where(d => d.DataSheetItemId == itemId && d.DataSheet.Product.CorpClientId == corpClientId).FirstOrDefaultAsync();
            context.DataSheetItems.Remove(data);
            await context.SaveChangesAsync(); ;
        }
    }
}
