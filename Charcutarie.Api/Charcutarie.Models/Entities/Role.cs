using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public List<RoleModule> RoleModules { get; set; }
    }
}
