using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IDataSheetItemRepository
    {
        Task<long> Add(NewDataSheetItem item, int corpClientId);
        Task<IEnumerable<DataSheetItem>> AddRange(IEnumerable<NewDataSheetItem> items, int corpClientId);
        Task<DataSheetItem> Get(long productId, int corpClientId);
        Task Update(UpdateDataSheetItem item, int corpClientId);
        Task Delete(long itemId, int corpClientId);
    }
}