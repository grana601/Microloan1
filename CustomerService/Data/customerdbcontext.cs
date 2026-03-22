using CustomerService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data;

public class customerdbcontext : DbContext
{
    public customerdbcontext(DbContextOptions<customerdbcontext> options) : base(options) { }

    public DbSet<customerprofile> customerprofile { get; set; } = null!;
    public DbSet<customeraddress> customeraddress { get; set; } = null!;
    public DbSet<customerbank> customerbank { get; set; } = null!;
    public DbSet<customerincome> customerincome { get; set; } = null!;
    public DbSet<customerdebt> customerdebt { get; set; } = null!;
    public DbSet<customerdocument> customerdocument { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<customerprofile>().ToTable("customerprofile");
        modelbuilder.Entity<customeraddress>().ToTable("customeraddress");
        modelbuilder.Entity<customerbank>().ToTable("customerbank");
        modelbuilder.Entity<customerincome>().ToTable("customerincome");
        modelbuilder.Entity<customerdebt>().ToTable("customerdebt");
        modelbuilder.Entity<customerdocument>().ToTable("customerdocument");
    }
}
