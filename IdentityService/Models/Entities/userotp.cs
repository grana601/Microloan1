namespace IdentityService.Models.Entities
{
    public class userotp : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string emailormobile { get; set; } = null!;
        public string otpcode { get; set; } = null!;
        public string purpose { get; set; } = null!;
        public bool isverified { get; set; }
    }
}
