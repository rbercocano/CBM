using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class SaveDataSheet
    {
        public long ProductId { get; set; }
        public string ProcedureDescription { get; set; }
        public double WeightLossPercentage { get; set; }
    }
}
