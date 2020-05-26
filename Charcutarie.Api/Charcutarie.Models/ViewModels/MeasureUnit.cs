using Charcutarie.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class MeasureUnit
    {
        public MeasureUnitEnum MeasureUnitId { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
    }
}
