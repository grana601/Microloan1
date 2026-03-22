namespace IdentityService.Models.DTOs
{
    public class AuthResponseDto
    {
        public string accesstoken { get; set; } = null!;
        public string refreshtoken { get; set; } = null!;
    }
}
