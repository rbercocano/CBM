using System;

namespace Charcutarie.Models.Entities
{
    public class User
    {
        public long UserId { get; set; }
        public int? CorpClientId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public CorpClient CorpClient { get; set; }
    }
}
