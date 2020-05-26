using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class PaymentStatus
    {
        public PaymentStatus()
        {
            Orders = new List<Order>();
        }
        public PaymentStatusEnum PaymentStatusId { get; set; }
        public string Description { get; set; }
        public List<Order> Orders { get; set; }
    }
}
