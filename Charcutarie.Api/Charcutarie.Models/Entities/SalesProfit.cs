using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class SalesSummary
    {
        public Guid RowId { get; set; }
        public decimal TotalSales { get; set; }
        public decimal CurrentMonthSales { get; set; }
    }
}
