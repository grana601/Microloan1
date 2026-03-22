namespace IdentityService.Models.DTOs
{
    public class RefreshTokenRequestDto
    {
        public string accesstoken { get; set; } = null!;
        public string refreshtoken { get; set; } = null!;
    }
}
