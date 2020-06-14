using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class SalesSummary
    {
        public Guid RowId { get; set; }
        public double TotalSales { get; set; }
        public double CurrentMonthSales { get; set; }
    }
}
