using Charcutarie.Models.Enums;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class NewProduct
    {
        public string Name { get; set; }
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public double Price { get; set; }
        public bool ActiveForSale { get; set; }
        [JsonIgnore]
        public int? CorpClientId { get; set; }
    }
}
