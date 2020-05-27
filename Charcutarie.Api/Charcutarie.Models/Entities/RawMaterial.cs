using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class RawMaterial
    {
        public long RawMaterialId { get; set; }
        public string Name { get; set; }
        public double PricePerGram { get; set; }
        public int CorpClientId { get; set; }
        public int MeasureUnitId { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
