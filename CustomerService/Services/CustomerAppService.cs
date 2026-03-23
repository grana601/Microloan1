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

    public async Task<Guid?> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var customer = new customers
        {
            id = Guid.NewGuid(),
            userid = Guid.NewGuid(), // Assuming no specific UserId is passed, generate one or update logic
            firstname = dto.firstname,
            lastname = dto.lastname,
            email = dto.email,
            phone = dto.phone,
            createdat = DateTime.UtcNow
        };
        _dbContext.customers.Add(customer);
        await _dbContext.SaveChangesAsync();
        return customer.id;
    }

    public async Task<bool> CreateAddressAsync(CustomerAddressRequest request)
    {
        var address = new customeraddress
        {
            id = Guid.NewGuid(),
            customerid = request.CustomerId,
            line1 = request.Address.street,
            city = request.Address.city,
            state = request.Address.state,
            country = request.Address.country,
            postcode = request.Address.zipcode,
            createdat = DateTime.UtcNow
        };
        _dbContext.customeraddress.Add(address);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateBankAsync(CustomerBankRequest request)
    {
        var bank = new customerbank
        {
            id = Guid.NewGuid(),
            customerid = request.CustomerId,
            accountholdername = "Unknown", // Add if LoanService sends
            bankname = request.BankDetails.bankname,
            accountnumber = request.BankDetails.accountnumber,
            sortcode = request.BankDetails.routingnumber,
            createdat = DateTime.UtcNow
        };
        _dbContext.customerbank.Add(bank);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateEmploymentAsync(CustomerEmploymentRequest request)
    {
        var employment = new customeremployment
        {
            id = Guid.NewGuid(),
            customerid = request.CustomerId,
            employmentstatus = request.EmploymentDetails.jobtitle, // fallback mapping
            incomesource = request.EmploymentDetails.employername, // fallback mapping
            monthlyincome = request.EmploymentDetails.monthlyincome,
            createdat = DateTime.UtcNow
        };
        _dbContext.customeremployment.Add(employment);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreateDebtAsync(CustomerDebtRequest request)
    {
        var debt = new customerdebt
        {
            id = Guid.NewGuid(),
            customerid = request.CustomerId,
            debttype = "Total Debt",
            amount = request.DebtInfo.totaldebt,
            createdat = DateTime.UtcNow
        };
        _dbContext.customerdebt.Add(debt);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

