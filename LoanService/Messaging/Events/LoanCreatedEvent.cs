namespace LoanService.Messaging.Events;

public class LoanCreatedEvent
{
    public Guid loanid { get; set; }
    public Guid customerid { get; set; }
    public decimal amount { get; set; }
    public DateTime createdat { get; set; }
}
