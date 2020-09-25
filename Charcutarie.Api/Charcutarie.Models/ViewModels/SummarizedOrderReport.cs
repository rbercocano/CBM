using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class SummarizedOrderReport
    {
        public long ProductId { get; set; }
        public string Product { get; set; }
        public int OrderItemStatusId { get; set; }
        public string OrderItemStatus { get; set; }
        public double Quantity { get; set; }
        public int MeasureUnitTypeId { get; set; }
        public int MeasureUnitId { get; set; }
        public string MeasureUnit { get; set; }
        public string ShortMeasureUnit { get; set; }
    }
}