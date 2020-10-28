using Charcutarie.Models.Enums;
using System;

namespace Charcutarie.Models.ViewModels
{
    public class UpdateUser 
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public MeasureUnitEnum DefaultMassMid { get; set; }
        public MeasureUnitEnum DefaultVolumeMid { get; set; }
        public string TimeZoneId { get; set; }
        
    }
}
