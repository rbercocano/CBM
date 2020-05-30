using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class DataSheet
    {
        public long DataSheetId { get; set; }
        public long ProductId { get; set; }
        public string ProcedureDescription { get; set; }
        public List<DataSheetItem> DataSheetItems { get; set; }
        public Product Product { get; set; }
    }
}
