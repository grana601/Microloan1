using System;

namespace LoanService.Models.Entities
{
    public class loanaction : baseentity<long>
    {
        public long loanid { get; set; }
        public string actiontype { get; set; } = null!;
        public string notes { get; set; } = null!;
        public DateTime timestamp { get; set; }
    }
}
