using Charcutarie.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charcutarie.Services.Contracts
{
    public interface IDomainService
    {
        Task<IEnumerable<CustomerType>> GetAllCustomerTypes();
        Task<IEnumerable<MeasureUnit>> GetAllMeasureUnits();
        Task<IEnumerable<Role>> GetAllRoles();
        Task<IEnumerable<ContactType>> GetAllContactTypes();
        Task<IEnumerable<PaymentStatus>> GetAllPaymentStatus();
        Task<IEnumerable<OrderItemStatus>> GetAllOrderItemStatus();
        Task<IEnumerable<OrderStatus>> GetAllOrderStatus();
    }
}
