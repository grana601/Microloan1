using IdentityService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class identitydbcontext : DbContext
{
    public identitydbcontext(DbContextOptions<identitydbcontext> options) : base(options)
    {
    }

    public DbSet<user> user { get; set; } = null!;
    public DbSet<role> role { get; set; } = null!;
    public DbSet<userrole> userrole { get; set; } = null!;
    public DbSet<userotp> userotp { get; set; } = null!;
    public DbSet<menu> menu { get; set; } = null!;
    public DbSet<menuinrole> menuinrole { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<user>().ToTable("user");
        modelbuilder.Entity<role>().ToTable("role");
        modelbuilder.Entity<userrole>().ToTable("userrole");
        modelbuilder.Entity<userotp>().ToTable("userotp");
        modelbuilder.Entity<menu>().ToTable("menu");
        modelbuilder.Entity<menuinrole>().ToTable("menuinrole");
    }
}
