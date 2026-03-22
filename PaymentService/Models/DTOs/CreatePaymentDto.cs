namespace PaymentService.Models.DTOs;

public class CreatePaymentDto
{
    public Guid LoanId { get; set; }
    public decimal Amount { get; set; }
}
