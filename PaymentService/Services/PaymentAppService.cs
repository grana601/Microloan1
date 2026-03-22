using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Interfaces;
using PaymentService.Models.DTOs;
using PaymentService.Models.Entities;

namespace PaymentService.Services;

public class PaymentAppService : IPaymentService
{
    private readonly PaymentServiceDbContext _dbContext;

    public PaymentAppService(PaymentServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreatePaymentAsync(CreatePaymentDto dto)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            LoanId = dto.LoanId,
            Amount = dto.Amount,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();

        return payment.Id;
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(Guid id)
    {
        var payment = await _dbContext.Payments.FindAsync(id);
        if (payment == null) return null;

        return new PaymentDto
        {
            Id = payment.Id,
            LoanId = payment.LoanId,
            Amount = payment.Amount,
            Status = payment.Status
        };
    }
}
