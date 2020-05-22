using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class SystemModule
    {
        public int SystemModuleId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public bool Active { get; set; }
        public SystemModule ParentModule { get; set; }
        public List<SystemModule> ChildModules { get; set; }
        public List<RoleModule> RoleModules { get; set; }
        public int Order { get; set; }
    }
}
