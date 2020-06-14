using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class DataSheet
    {
        public DataSheet()
        {
            DataSheetItems = new List<DataSheetItem>();
        }
        public long DataSheetId { get; set; }
        public long ProductId { get; set; }
        public string ProcedureDescription { get; set; }
        public List<DataSheetItem> DataSheetItems { get; set; }
        public double WeightLossPercentage { get; set; }
    }
}
