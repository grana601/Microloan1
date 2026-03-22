using LoanService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Data;

public class LoanServiceDbContext : DbContext
{
    public LoanServiceDbContext(DbContextOptions<LoanServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Loan> Loans { get; set; }
}
