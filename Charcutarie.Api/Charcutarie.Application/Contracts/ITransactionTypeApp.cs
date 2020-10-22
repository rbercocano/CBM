using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ITransactionTypeApp
    {
        Task<IEnumerable<TransactionType>> GetAll();
        Task<TransactionType> Get(int id);
    }
}