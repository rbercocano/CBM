using AutoMapper;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using Charcutarie.Repository.DbContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF = Charcutarie.Models.Entities;

namespace Charcutarie.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CharcuterieDbContext context;
        private readonly IMapper mapper;

        public TransactionRepository(CharcuterieDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public IEnumerable<Balance> GetBalance(DateTime start, DateTime end, int corpClientId)
        {
            var @params = new[]
            {
                new SqlParameter("@start",start){SqlDbType = System.Data.SqlDbType.Date},
                new SqlParameter("@end",end){SqlDbType = System.Data.SqlDbType.Date},
                new SqlParameter("@cid",corpClientId){SqlDbType = System.Data.SqlDbType.Int}
            };
            var data = context.Balance.FromSqlRaw(@"EXEC [dbo].[GetBalance]   
                                                            @start = @start,
                                                            @end = @end,
                                                            @cid = @cid", @params);
            var result = mapper.Map<IEnumerable<Balance>>(data);
            return result;
        }
        public decimal GetTotalBalance(int corpClientId)
        {
            var total = context.Transactions.Where(t => t.CorpClientId == corpClientId).Sum(t => t.Amount);
            return total;
        }
        public async Task<Transaction> AddTransaction(NewTransaction transaction)
        {
            var newT = mapper.Map<EF.Transaction>(transaction);
            newT.Date = newT.Date.ToUniversalTime();
            var trans = context.Transactions.Add(newT);
            await context.SaveChangesAsync();
            return mapper.Map<Transaction>(newT);
        }
        public async Task RemoveTransaction(int corpClientId, long transactionId)
        {
            var trans = context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId && t.CorpClientId == corpClientId);
            context.Transactions.Remove(trans);
            await context.SaveChangesAsync();
        }
        public async Task RemoveTransactionByOrderId(int corpClientId, long orderId)
        {
            var trans = context.Transactions.Where(t => t.OrderId == orderId && t.CorpClientId == corpClientId);
            context.Transactions.RemoveRange(trans);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(long? orderId, int corpClientId)
        {
            var trans = context.Transactions.Where(t => t.CorpClientId == corpClientId);
            if (orderId.HasValue)
                trans = trans.Where(t => t.OrderId == orderId);

            var result = await trans.ToListAsync();
            return mapper.Map<IEnumerable<Transaction>>(result);
        }

        public async Task<Transaction> GetTransaction(long transactionId, int corpClientId)
        {
            var trans = await context.Transactions.FirstOrDefaultAsync(t => t.CorpClientId == corpClientId && t.TransactionId == transactionId);
            return mapper.Map<Transaction>(trans);
        }

        public async Task ChangeStatus(long transactionId, int corpClientId, int status)
        {
            var trans = await context.Transactions.FirstOrDefaultAsync(t => t.CorpClientId == corpClientId && t.TransactionId == transactionId);
            trans.TransactionStatusId = status;
            context.Transactions.Update(trans);
            context.SaveChanges();
        }
    }
}
