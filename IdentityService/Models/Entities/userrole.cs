namespace IdentityService.Models.Entities
{
    public class userrole : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string roleid { get; set; } = null!;
    }
}
