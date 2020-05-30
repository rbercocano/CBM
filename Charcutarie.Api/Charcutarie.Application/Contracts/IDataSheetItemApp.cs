using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IDataSheetItemApp
    {
        Task<long> Add(NewDataSheetItem item, int corpClientId);
        Task<IEnumerable<DataSheetItem>> AddRange(IEnumerable<NewDataSheetItem> items, int corpClientId);
        Task<DataSheetItem> Get(long itemId, int corpClientId);
        Task Update(UpdateDataSheetItem item, int corpClientId);
        Task Delete(long itemId, int corpClientId);
    }
}