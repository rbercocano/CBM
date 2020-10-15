using System;

namespace Charcutarie.Models.ViewModels
{
    public class Product : UpdateProduct
    {
        private double? cost;

        public string MeasureUnit { get; set; }
        public new int CorpClientId { get; set; }
        public double? Cost
        {
            get => cost;
            set
            {
                cost = value == 0 ? null : value;
            }
        }
        public double? Profit
        {
            get
            {
                if (!Cost.HasValue || Cost == 0) return null;
                return Price - Cost.Value;
            }
        }
        public double? ProfitPercentage
        {
            get
            {
                if (!Cost.HasValue || Cost == 0) return null;
                var perc = (Price - Cost.Value) / Cost * 100;
                return (double)Math.Round((decimal)perc, 2);
            }
        }
    }
}
