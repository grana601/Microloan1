using Microsoft.EntityFrameworkCore;
using PaymentService.Models.Entities;

namespace PaymentService.Data;

public class PaymentServiceDbContext : DbContext
{
    public PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }
}
