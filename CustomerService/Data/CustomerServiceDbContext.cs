using CustomerService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data;

public class CustomerServiceDbContext : DbContext
{
    public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { set; get; } = null!;
}
