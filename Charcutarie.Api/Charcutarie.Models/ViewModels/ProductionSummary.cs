using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ProductionSummary
    {
        public ProductionSummary()
        {
            ProductionItems = new List<ProductionItem>();
        }
        public decimal ProductionCost { get { return Math.Round(ProductionItems.Sum(i => i.Cost), 2); } }
        public decimal SalePrice { get; set; }
        public decimal Profit { get { return Math.Round(SalePrice - ProductionCost, 2); } }
        public decimal ProfitPercentage { get { return Math.Round(Profit / ProductionCost * 100, 2); } }
        public List<ProductionItem> ProductionItems { get; set; }
    }
}
