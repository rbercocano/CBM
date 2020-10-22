using Charcutarie.Application.Contracts;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services
{
    public class DomainService : IDomainService
    {
        private readonly IMeasureUnitApp measureUnitApp;
        private readonly ICustomerTypeApp customerTypeApp;
        private readonly IRoleApp roleApp;
        private readonly IContactTypeApp contactTypeApp;
        private readonly IOrderStatusApp orderStatusApp;
        private readonly IOrderItemStatusApp orderItemStatusApp;
        private readonly IPaymentStatusApp paymentStatus;
        private readonly ITransactionTypeApp transactionTypeApp;

        public DomainService(IMeasureUnitApp measureUnitApp,
            ICustomerTypeApp customerTypeApp,
            IRoleApp roleApp,
            IContactTypeApp contactTypeApp,
            IOrderStatusApp orderStatusApp,
            IOrderItemStatusApp orderItemStatusApp,
            IPaymentStatusApp paymentStatus,
            ITransactionTypeApp transactionTypeApp)
        {
            this.measureUnitApp = measureUnitApp;
            this.customerTypeApp = customerTypeApp;
            this.roleApp = roleApp;
            this.contactTypeApp = contactTypeApp;
            this.orderStatusApp = orderStatusApp;
            this.orderItemStatusApp = orderItemStatusApp;
            this.paymentStatus = paymentStatus;
            this.transactionTypeApp = transactionTypeApp;
        }
        public async Task<IEnumerable<CustomerType>> GetAllCustomerTypes()
        {
            return await customerTypeApp.GetAll();
        }

        public async Task<IEnumerable<MeasureUnit>> GetAllMeasureUnits()
        {
            return await measureUnitApp.GetAll();
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await roleApp.GetAll();
        }
        public async Task<IEnumerable<ContactType>> GetAllContactTypes()
        {
            return await contactTypeApp.GetAll();
        }
        public async Task<IEnumerable<OrderStatus>> GetAllOrderStatus()
        {
            return await orderStatusApp.GetAll();
        }
        public async Task<IEnumerable<OrderItemStatus>> GetAllOrderItemStatus()
        {
            return await orderItemStatusApp.GetAll();
        }
        public async Task<IEnumerable<PaymentStatus>> GetAllPaymentStatus()
        {
            return await paymentStatus.GetAll();
        }
        public async Task<IEnumerable<TransactionType>> GetAllTransactionTypes()
        {
            return await transactionTypeApp.GetAll();
        }
    }
}
