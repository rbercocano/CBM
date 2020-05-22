using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Repository.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Application
{
    public class PaymentStatusApp : IPaymentStatusApp
    {
        private readonly IPaymentStatusRepository repository;

        public PaymentStatusApp(IPaymentStatusRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<PaymentStatus>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
