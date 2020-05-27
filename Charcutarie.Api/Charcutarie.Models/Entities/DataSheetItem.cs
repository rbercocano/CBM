using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class DataSheetItem
    {
        public long DataSheetItemId { get; set; }
        public long DataSheetId { get; set; }
        public long RawMaterialId { get; set; }
        public double Percentage { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
