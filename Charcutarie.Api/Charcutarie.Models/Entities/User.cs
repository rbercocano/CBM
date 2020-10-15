using Charcutarie.Models.Enums;
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
        public string Email { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public MeasureUnit MassMeasureUnit { get; set; }
        public MeasureUnit VolumeMeasureUnit { get; set; }
        public MeasureUnitEnum DefaultMassMid { get; set; }
        public MeasureUnitEnum DefaultVolumeMid { get; set; }
    }
}
