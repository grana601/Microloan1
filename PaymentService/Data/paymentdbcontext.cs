using PaymentService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Data;

public class paymentdbcontext : DbContext
{
    public paymentdbcontext(DbContextOptions<paymentdbcontext> options) : base(options) { }

    public DbSet<repaymentschedule> repaymentschedule { get; set; } = null!;
    public DbSet<disbursement> disbursement { get; set; } = null!;
    public DbSet<paymenttransaction> paymenttransaction { get; set; } = null!;
    public DbSet<stripeintegration> stripeintegration { get; set; } = null!;
    public DbSet<gatewaylog> gatewaylog { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<repaymentschedule>().ToTable("repaymentschedule");
        modelbuilder.Entity<disbursement>().ToTable("disbursement");
        modelbuilder.Entity<paymenttransaction>().ToTable("paymenttransaction");
        modelbuilder.Entity<stripeintegration>().ToTable("stripeintegration");
        modelbuilder.Entity<gatewaylog>().ToTable("gatewaylog");
    }
}
