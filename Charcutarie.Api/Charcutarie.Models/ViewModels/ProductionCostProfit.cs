using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ProductionCostProfit
    {
        public long ProductId { get; set; }
        public string Product { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }
        public double ProfitPercentage { get; set; }
    }
}
