using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class OrderCountSummary
    {
        public Guid RowId { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCompletedOrders { get; set; }
    }
}
