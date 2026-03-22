using System;

namespace LoanService.Models.Entities
{
    public class loans
    {
        public Guid id { get; set; }
        public string customerid { get; set; } = null!;
        public decimal amount { get; set; }
        public int termmonths { get; set; }
        public decimal interestrate { get; set; }
        public string status { get; set; } = null!;
        public string purpose { get; set; } = null!;
        public DateTime createdat { get; set; }
        public DateTime updatedat { get; set; }
    }
}
