using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IDataSheetService
    {
        Task<long> AddItem(NewDataSheetItem item, int corpClientId);
        Task<long> Create(DataSheet dataSheet, int corpClientId);
        Task DeleteItem(long itemId, int corpClientId);
        Task<DataSheet> GetDataSheet(long productId, int corpClientId);
        Task<DataSheetItem> GetDataSheetItem(long itemId, int corpClientId);
        Task<long> Update(SaveDataSheet saveDataSheet, int corpClientId);
        Task UpdateItem(UpdateDataSheetItem item, int corpClientId);
        Task<ProductionSummary> CalculateProduction(long productId, MeasureUnitEnum measureId, decimal quantity, int corpClientId);
    }
}