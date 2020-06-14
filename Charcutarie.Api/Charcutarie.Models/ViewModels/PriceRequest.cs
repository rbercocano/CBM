using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PriceRequest
    {
        public double ProductPrice { get; set; }
        public MeasureUnitEnum ProductMeasureUnit { get; set; }
        public double Quantity { get; set; }
        public MeasureUnitEnum QuantityMeasureUnit { get; set; }
        private int resultPrecision =2;
        public int ResultPrecision { get { return resultPrecision; } set { resultPrecision = value == 0 ? 2 : value; } }
    }
}
