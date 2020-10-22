using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Repository.Contracts
{
    public interface ITransactionTypeRepository
    {
        Task<IEnumerable<TransactionType>> GetAll();
        Task<TransactionType> Get(int id);
    }
}