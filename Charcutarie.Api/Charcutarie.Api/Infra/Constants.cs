using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charcutarie.Api.Infra
{
    public static class Constants
    {
        public const string JWTRole = "Role";
        public static readonly string[] JWTAllowedRoles = new[] { "Customer", "SysAdmin" };
    }
}
