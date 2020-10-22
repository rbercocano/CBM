using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewRawMaterial
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public int CorpClientId { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
    }
}
