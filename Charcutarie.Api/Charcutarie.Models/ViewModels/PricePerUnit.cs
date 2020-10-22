using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class PricePerUnit
    {
        public decimal Price { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
    }
}
