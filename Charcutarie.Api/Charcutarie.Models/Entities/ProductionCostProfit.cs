using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
   public class ProductionCostProfit
    {
        public long ProductId { get; set; }
        public string Product { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPercentage { get; set; }
    }
}
