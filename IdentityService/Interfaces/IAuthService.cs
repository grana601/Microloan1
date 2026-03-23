using System.Threading.Tasks;
using IdentityService.Models.DTOs;

namespace IdentityService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task<bool> LogoutAsync(string userid);
        Task<bool> CheckUserAsync(string username);
        Task<bool> AssignRoleAsync(string username, string role);
    }
}
