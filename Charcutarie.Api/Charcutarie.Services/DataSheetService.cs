using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class DataSheetService : IDataSheetService
    {
        private readonly IDataSheetApp dataSheetApp;
        private readonly IDataSheetItemApp dataSheetItemApp;

        public DataSheetService(IDataSheetApp dataSheetApp, IDataSheetItemApp dataSheetItemApp)
        {
            this.dataSheetApp = dataSheetApp;
            this.dataSheetItemApp = dataSheetItemApp;
        }
        public async Task<long> Create(DataSheet dataSheet, int corpClientId)
        {
            var result = await dataSheetApp.Save(new SaveDataSheet { ProcedureDescription = dataSheet.ProcedureDescription, ProductId = dataSheet.ProductId }, corpClientId);
            var items = dataSheet.DataSheetItems.Select(i => new NewDataSheetItem
            {
                ProductId = dataSheet.ProductId,
                AdditionalInfo = i.AdditionalInfo,
                Percentage = i.Percentage,
                RawMaterialId = i.RawMaterialId
            }).ToList();
            if (items.Any())
                await dataSheetItemApp.AddRange(items, corpClientId);
            return result.DataSheetId;
        }
        public async Task<long> Update(SaveDataSheet saveDataSheet, int corpClientId)
        {
            var result = await dataSheetApp.Save(saveDataSheet, corpClientId);
            return result.DataSheetId;
        }
        public async Task<long> AddItem(NewDataSheetItem item, int corpClientId)
        {
            var result = await dataSheetItemApp.Add(item, corpClientId);
            return result;
        }
        public async Task UpdateItem(UpdateDataSheetItem item, int corpClientId)
        {
            await dataSheetItemApp.Update(item, corpClientId);
        }
        public async Task DeleteItem(long itemId, int corpClientId)
        {
            await dataSheetItemApp.Delete(itemId, corpClientId);
        }
        public async Task<DataSheetItem> GetDataSheetItem(long itemId, int corpClientId)
        {
            return await dataSheetItemApp.Get(itemId, corpClientId);
        }
        public async Task<DataSheet> GetDataSheet(long productId, int corpClientId)
        {
            return await dataSheetApp.Get(productId, corpClientId);
        }
    }
}
