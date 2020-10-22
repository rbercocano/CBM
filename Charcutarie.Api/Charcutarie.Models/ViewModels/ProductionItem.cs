using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ProductionItem : DataSheetItem
    {
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public MeasureUnit MeasureUnit { get; set; }

    }
}
