using LoanService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Data;

public class loandbcontext : DbContext
{
    public loandbcontext(DbContextOptions<loandbcontext> options) : base(options) { }

    public DbSet<loan> loan { get; set; } = null!;
    public DbSet<loanagreement> loanagreement { get; set; } = null!;
    public DbSet<loanproductconfig> loanproductconfig { get; set; } = null!;
    public DbSet<loannote> loannote { get; set; } = null!;
    public DbSet<loanaction> loanaction { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<loan>().ToTable("loan");
        modelbuilder.Entity<loanagreement>().ToTable("loanagreement");
        modelbuilder.Entity<loanproductconfig>().ToTable("loanproductconfig");
        modelbuilder.Entity<loannote>().ToTable("loannote");
        modelbuilder.Entity<loanaction>().ToTable("loanaction");
    }
}
