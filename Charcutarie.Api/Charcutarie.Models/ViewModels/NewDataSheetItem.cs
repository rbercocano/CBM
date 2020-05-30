using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class NewDataSheetItem
    {
        public long ProductId { get; set; }
        public long RawMaterialId { get; set; }
        public double Percentage { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
