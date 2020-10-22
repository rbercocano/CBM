using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionApp app;

        public TransactionService(ITransactionApp app)
        {
            this.app = app;
        }

        public async Task<Transaction> AddTransaction(NewTransaction transaction, long userId)
        {
            return await app.AddTransaction(transaction, userId);
        }

        public IEnumerable<Balance> GetBalance(DateTime start, DateTime end, int corpClientId)
        {
            return app.GetBalance(start, end, corpClientId);
        }

        public decimal GetTotalBalance(int corpClientId)
        {
            return app.GetTotalBalance(corpClientId);
        }

        public async Task RemoveTransaction(int corpClientId, long transactionId)
        {
            await app.RemoveTransaction(corpClientId, transactionId);
        }

    }
}
