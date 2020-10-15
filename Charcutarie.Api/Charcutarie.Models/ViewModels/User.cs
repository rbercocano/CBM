using Charcutarie.Models.Enums;
using System;
using System.Text.Json.Serialization;

namespace Charcutarie.Models.ViewModels
{
    public class User
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? CorpClientId { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public MeasureUnitEnum DefaultMassMid { get; set; }
        public MeasureUnitEnum DefaultVolumeMid { get; set; }
    }
}
