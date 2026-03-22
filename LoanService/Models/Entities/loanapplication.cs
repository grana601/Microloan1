using System;

namespace LoanService.Models.Entities
{
    public class loanapplication
    {
        public Guid id { get; set; }
        public string customerid { get; set; } = null!;
        public decimal requestedamount { get; set; }
        public string status { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
