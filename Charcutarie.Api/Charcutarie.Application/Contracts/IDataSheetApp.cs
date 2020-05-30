using Charcutarie.Models.ViewModels;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface IDataSheetApp
    {
        Task<DataSheet> Get(long productId, int corpClientId);
        Task<DataSheet> Save(SaveDataSheet dataSheet, int corpClientId);
    }
}