using PaymentService.Models.DTOs;

namespace PaymentService.Interfaces;

public interface IPaymentService
{
    Task<Guid> CreatePaymentAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetPaymentByIdAsync(Guid id);
}
