namespace PaymentService.Models.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid LoanId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}
