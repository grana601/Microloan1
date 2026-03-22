namespace IdentityService.Models.DTOs
{
    public class LoginRequestDto
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
