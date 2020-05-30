using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface IDataSheetRepository
    {
        Task<DataSheet> Get(long productId, int corpClientId);
        Task<DataSheet> Save(SaveDataSheet dataSheet, int corpClientId);
    }
}