using System;
using System.Collections.Generic;
using System.Text;

namespace Charcutarie.Models.ViewModels
{
    public class JWTUserInfo
    {
        public long UserId { get; set; }
        public int? CorpClientId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string DBAName { get; set; }
        public string CompanyName { get; set; }
    }
}
