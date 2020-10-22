using System;

namespace Charcutarie.Models.ViewModels
{
    public class Product : UpdateProduct
    {
        private decimal? cost;

        public string MeasureUnit { get; set; }
        public new int CorpClientId { get; set; }
        public decimal? Cost
        {
            get => cost;
            set
            {
                cost = value == 0 ? null : value;
            }
        }
        public decimal? Profit
        {
            get
            {
                if (!Cost.HasValue || Cost == 0) return null;
                return Price - Cost.Value;
            }
        }
        public decimal? ProfitPercentage
        {
            get
            {
                if (!Cost.HasValue || Cost == 0) return null;
                var perc = (Price - Cost.Value) / Cost * 100;
                return (decimal)Math.Round((decimal)perc, 2);
            }
        }
    }
}
