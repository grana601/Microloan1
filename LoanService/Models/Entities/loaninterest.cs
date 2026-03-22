using System;

namespace LoanService.Models.Entities
{
    public class loaninterest
    {
        public Guid id { get; set; }
        public decimal rate { get; set; }
        public string type { get; set; } = null!; // monthly/yearly
        public DateTime effectivefrom { get; set; }
    }
}
