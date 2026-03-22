using IdentityService.Models.DTOs;

namespace IdentityService.Interfaces;

public interface IIdentityAppService
{
    Task<Guid> RegisterUserAsync(RegisterUserDto dto);
    Task<UserDto?> GetUserByIdAsync(Guid id);
}
