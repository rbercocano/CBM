using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ProfitSummary
    {
        public Guid RowId { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal CurrentMonthProfit { get; set; }
    }
}
