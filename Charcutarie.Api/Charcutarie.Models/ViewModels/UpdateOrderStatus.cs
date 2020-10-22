using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateOrderStatus
    {
        public OrderStatusEnum OrderStatusId { get; set; }
        public long OrderNumber { get; set; }
    }
}
