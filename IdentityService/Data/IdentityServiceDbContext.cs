using IdentityService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class IdentityServiceDbContext : DbContext
{
    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}
