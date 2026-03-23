namespace IdentityService.Models.DTOs
{
    public class RegisterRequestDto
    {
        public string username { get; set; } = null!;
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? phone { get; set; }
    }
}
