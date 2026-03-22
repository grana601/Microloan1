using CustomerService.Data;
using CustomerService.Interfaces;
using CustomerService.Models.DTOs;
using CustomerService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Services;

public class CustomerAppService : ICustomerService
{
    private readonly CustomerServiceDbContext _dbContext;

    public CustomerAppService(CustomerServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return customer.Id;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        };
    }
}
