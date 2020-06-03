using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class ParentModule
    {
        public ParentModule()
        {
            ChildModules = new List<ChildModule>();
        }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string Route { get; set; }
        public bool IsMenu { get; set; }
        public List<ChildModule> ChildModules { get; set; }
    }
}
