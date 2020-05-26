using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new List<Order>();
        }
        public OrderStatusEnum OrderStatusId { get; set; }
        public string Description { get; set; }
        public List<Order> Orders { get; set; }
    }
}
