using CustomerService.Data;
using CustomerService.Interfaces;
using CustomerService.Models.DTOs;
using CustomerService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Services;

public class CustomerAppService : ICustomerService
{
    private readonly customerdbcontext _dbContext;

    public CustomerAppService(customerdbcontext dbContext)
    {
        _dbContext = dbContext;
    }

  
}
