
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class OrderSummary
    {
        public long OrderId { get; set; }
        public long orderNumber { get; set; }
        public int CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string SocialIdentifier { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CompleteBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PaidOn { get; set; }
        public decimal FinalPrice { get; set; }
        public long CustomerId { get; set; }
    }
}
