using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class TransactionTypeApp : ITransactionTypeApp
    {
        private readonly ITransactionTypeRepository repository;

        public TransactionTypeApp(ITransactionTypeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TransactionType> Get(int id)
        {
            return await repository.Get(id);
        }

        public async Task<IEnumerable<TransactionType>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
