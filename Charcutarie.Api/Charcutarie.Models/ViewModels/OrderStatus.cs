using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class OrderStatus
    {
        public OrderStatusEnum OrderStatusId { get; set; }
        public string Description { get; set; }
    }
}
