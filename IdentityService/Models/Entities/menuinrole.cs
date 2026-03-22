namespace IdentityService.Models.Entities
{
    public class menuinrole : baseentity<long>
    {
        public long menuid { get; set; }
        public string roleid { get; set; } = null!;
    }
}
