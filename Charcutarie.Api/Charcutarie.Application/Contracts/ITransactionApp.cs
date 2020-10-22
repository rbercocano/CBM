using Charcutarie.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Charcutarie.Application.Contracts
{
    public interface ITransactionApp
    {
        Task<Transaction> AddTransaction(NewTransaction transaction, long userId);
        IEnumerable<Balance> GetBalance(DateTime start, DateTime end, int corpClientId);
        Task RemoveTransaction(int corpClientId, long transactionId);
        Task RemoveTransactionByOrderId(int corpClientId, long orderId);
        Task<IEnumerable<Transaction>> GetTransactions(long? orderId, int corpClientId);
        Task<Transaction> GetTransaction(long transactionId, int corpClientId);
        Task ChangeStatus(long transactionId, int corpClientId, int status);
        decimal GetTotalBalance(int corpClientId);
    }
}
