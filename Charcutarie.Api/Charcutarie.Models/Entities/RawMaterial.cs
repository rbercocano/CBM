using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class RawMaterial
    {
        public long RawMaterialId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CorpClientId { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
