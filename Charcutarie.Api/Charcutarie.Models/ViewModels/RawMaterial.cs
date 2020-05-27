using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class RawMaterial
    {
        public long RawMaterialId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CorpClientId { get; set; }
        public int MeasureUnitId { get; set; }
        public string MeasureUnit { get; set; }
    }
}
