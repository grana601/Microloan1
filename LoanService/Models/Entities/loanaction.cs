using System;

namespace LoanService.Models.Entities
{
    public class loanaction
    {
        public Guid id { get; set; }
        public Guid loanid { get; set; }
        public string actiontype { get; set; } = null!;
        public string notes { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
