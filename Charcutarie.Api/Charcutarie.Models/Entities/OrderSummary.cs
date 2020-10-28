using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class OrderSummary
    {
        public long OrderId { get; set; }
        public long OrderNumber { get; set; }
        public int CustomerTypeId { get; set; }        
        public string Name { get; set; }
        public string SocialIdentifier { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CompleteBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? PaidOn { get; set; }
        public decimal FinalPrice { get; set; }
        public long CustomerId { get; set; }
    }
}
