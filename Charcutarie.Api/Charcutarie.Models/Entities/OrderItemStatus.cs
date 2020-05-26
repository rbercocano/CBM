using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class OrderItemStatus
    {
        public OrderItemStatusEnum OrderItemStatusId { get; set; }
        public string Description { get; set; }
    }
}
