using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ChildModule
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public bool IsMenu { get; set; }
    }
}
