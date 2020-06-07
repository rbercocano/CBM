using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateOrderItem : NewOrderItem
    {
        public long OrderItemId { get; set; }
    }
}
