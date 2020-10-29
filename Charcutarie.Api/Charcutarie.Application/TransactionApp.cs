using Charcutarie.Application.Contracts;
using Charcutarie.Core.ExceptionHandling;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class TransactionApp : ITransactionApp
    {
        private readonly ITransactionRepository repository;
        private readonly ITransactionTypeApp transactionTypeApp;

        public TransactionApp(ITransactionRepository repository, ITransactionTypeApp transactionTypeApp)
        {
            this.repository = repository;
            this.transactionTypeApp = transactionTypeApp;
        }

        public async Task<Transaction> AddTransaction(NewTransaction transaction, long userId)
        {
            var type = await transactionTypeApp.Get(transaction.TransactionTypeId);
            transaction.Amount = type.Type == "I" ? transaction.Amount : transaction.Amount * -1;
            transaction.UserId = userId;
            return await repository.AddTransaction(transaction);
        }

        public IEnumerable<Balance> GetBalance(DateTimeOffset start, DateTimeOffset end, int corpClientId)
        {
            var totalDays = (end - start).TotalDays;
            if (totalDays > 180)
                throw new BusinessException("O período de busca não pode ser maior que 180 dias");
            return repository.GetBalance(start, end, corpClientId);
        }

        public async Task RemoveTransaction(int corpClientId, long transactionId)
        {
            await repository.RemoveTransaction(corpClientId, transactionId);
        }

        public async Task RemoveTransactionByOrderId(int corpClientId, long orderId)
        {
            await repository.RemoveTransactionByOrderId(corpClientId, orderId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(long? orderId, int corpClientId)
        {
            return await repository.GetTransactions(orderId, corpClientId);
        }

        public async Task<Transaction> GetTransaction(long transactionId, int corpClientId)
        {
            return await repository.GetTransaction(transactionId, corpClientId);
        }
        public async Task ChangeStatus(long transactionId, int corpClientId, int status)
        {
            await repository.ChangeStatus(transactionId, corpClientId, status);
        }

        public decimal GetTotalBalance(int corpClientId)
        {
            return repository.GetTotalBalance(corpClientId);
        }
    }
}
