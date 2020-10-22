using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class SalesPerMonth
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
