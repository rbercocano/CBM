using System;

namespace Charcutarie.Models.ViewModels
{
    public class Product : UpdateProduct
    {
        public string MeasureUnit { get; set; }
        public new int CorpClientId { get; set; }
        public double? Cost { get; set; }
        public double? Profit
        {
            get
            {
                if (!Cost.HasValue) return null;
                return Price - Cost.Value;
            }
        }
        public double? ProfitPercentage
        {
            get
            {
                if (!Cost.HasValue) return null;
                var perc = (Price - Cost.Value) / Cost * 100;
                return (double)Math.Round((decimal)perc, 2);
            }
        }
    }
}
