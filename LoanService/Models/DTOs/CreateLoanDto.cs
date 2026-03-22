namespace LoanService.Models.DTOs;

public class CreateLoanDto
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
}
