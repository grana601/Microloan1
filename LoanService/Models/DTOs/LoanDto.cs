namespace LoanService.Models.DTOs;

public class LoanDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public string Status { get; set; } = string.Empty;
}
