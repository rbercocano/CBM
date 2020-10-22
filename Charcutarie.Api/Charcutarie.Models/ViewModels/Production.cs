using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class Production
    {
        public long ProductId { get; set; }
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public MeasureUnitTypeEnum MeasureUnitTypeId { get; set; }
        public string MeasureUnit { get; set; }
    }
}
