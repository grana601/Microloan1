using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Models.DTOs;
using IdentityService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Services;

public class IdentityAppServiceImplementation : IIdentityAppService
{
    private readonly IdentityServiceDbContext _dbContext;

    public IdentityAppServiceImplementation(IdentityServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> RegisterUserAsync(RegisterUserDto dto)
    {
        // Basic implementation for learning purposes (not using real hashing)
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = "hashed_" + dto.Password, 
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }
}
