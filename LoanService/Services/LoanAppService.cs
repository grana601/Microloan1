using LoanService.Data;
using LoanService.Interfaces;
using LoanService.Models.DTOs;
using LoanService.Models.Entities;
using LoanService.Services.External;
using LoanService.Messaging;
using LoanService.Messaging.Events;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Services;

public class LoanAppService : ILoanService
{
    private readonly loandbcontext _dbContext;
    private readonly IIdentityServiceClient _identityServiceClient;
    private readonly ICustomerServiceClient _customerServiceClient;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public LoanAppService(
        loandbcontext dbContext,
        IIdentityServiceClient identityServiceClient,
        ICustomerServiceClient customerServiceClient,
        IRabbitMqPublisher rabbitMqPublisher)
    {
        _dbContext = dbContext;
        _identityServiceClient = identityServiceClient;
        _customerServiceClient = customerServiceClient;
        _rabbitMqPublisher = rabbitMqPublisher;
    }

    public async Task<bool> ApplyLoanAsync(ApplyLoanRequestDto dto)
    {
        // 1. Check user in IdentityService
        bool userExists = await _identityServiceClient.CheckUserAsync(dto.username);
        if (!userExists)
        {
            var registerRequest = new RegisterUserRequest
            {
                username = dto.username,
                email = dto.email,
                password = dto.password ?? "DefaultPassword123!",
                firstname = dto.firstname,
                lastname = dto.lastname,
                phone = dto.phone
            };
            await _identityServiceClient.RegisterUserAsync(registerRequest);
        }

        // 2. Assign role
        await _identityServiceClient.AssignRoleAsync(dto.username, "customer");

        // 3. Create customer + related data
        var createCustomerRequest = new CreateCustomerRequest
        {
            username = dto.username,
            email = dto.email,
            firstname = dto.firstname,
            lastname = dto.lastname,
            phone = dto.phone
        };
        var customerId = await _customerServiceClient.CreateCustomerAsync(createCustomerRequest);
        if (customerId == null)
        {
            return false; // Failed to create customer
        }

        await _customerServiceClient.CreateAddressAsync(customerId.Value, dto.address);
        await _customerServiceClient.CreateBankAsync(customerId.Value, dto.bankdetails);
        await _customerServiceClient.CreateEmploymentAsync(customerId.Value, dto.employmentdetails);
        await _customerServiceClient.CreateDebtAsync(customerId.Value, dto.debtinfo);

        // 4. Create loan in LoanService DB
        var loan = new loans
        {
            id = Guid.NewGuid(),
            customerid = customerId.Value,
            amount = dto.loanamount,
            termmonths = dto.termmonths,
            interestrate = 5.0m, // Default or calculate based on logic
            status = "pending",
            purpose = dto.purpose,
            createdat = DateTime.UtcNow
        };

        _dbContext.loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        // 5. Publish RabbitMQ event
        var loanCreatedEvent = new LoanCreatedEvent
        {
            loanid = loan.id,
            customerid = loan.customerid,
            amount = loan.amount,
            createdat = loan.createdat
        };

        var queueName = "LoanCreatedEvent"; // Load from config in a real app or keep it simple
        await _rabbitMqPublisher.PublishEventAsync(loanCreatedEvent, queueName);

        return true;
    }
    public async Task<List<loans>> GetLoanListAsync()
    {
        return await _dbContext.loans.ToListAsync();
    }
}

