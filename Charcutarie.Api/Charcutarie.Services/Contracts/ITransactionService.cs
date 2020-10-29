using Charcutarie.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(NewTransaction transaction, long userId);
        IEnumerable<Balance> GetBalance(DateTimeOffset start, DateTimeOffset end, int corpClientId);
        Task RemoveTransaction(int corpClientId, long transactionId);
        decimal GetTotalBalance(int corpClientId);
    }
}