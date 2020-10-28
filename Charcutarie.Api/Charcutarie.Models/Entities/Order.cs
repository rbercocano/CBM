using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class Order
    {
        public long OrderId { get; set; }
        public long OrderNumber { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CompleteBy { get; set; }
        public DateTime? PaidOn { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public PaymentStatusEnum PaymentStatusId { get; set; }
        public decimal FreightPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
