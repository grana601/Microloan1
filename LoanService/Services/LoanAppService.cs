using LoanService.Data;
using LoanService.Interfaces;
using LoanService.Models.DTOs;
using LoanService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Services;

public class LoanAppService : ILoanService
{
    private readonly LoanServiceDbContext _dbContext;

    public LoanAppService(LoanServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateLoanAsync(CreateLoanDto dto)
    {
        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            Amount = dto.Amount,
            InterestRate = dto.InterestRate,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan.Id;
    }

    public async Task<LoanDto?> GetLoanByIdAsync(Guid id)
    {
        var loan = await _dbContext.Loans.FindAsync(id);
        if (loan == null) return null;

        return new LoanDto
        {
            Id = loan.Id,
            CustomerId = loan.CustomerId,
            Amount = loan.Amount,
            InterestRate = loan.InterestRate,
            Status = loan.Status
        };
    }
}
