namespace LoanService.Models.Entities;

public class Loan
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
