using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateRawmaterial: NewRawMaterial
    {
        public long RawMaterialId { get; set; }
    }
}
