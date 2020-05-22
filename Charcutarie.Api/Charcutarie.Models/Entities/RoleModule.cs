namespace Charcutarie.Models.Entities
{
    public class RoleModule
    {
        public int RoleModuleId { get; set; }
        public int SystemModuleId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public SystemModule SystemModule { get; set; }
    }
}
