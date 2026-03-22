using System;

namespace LoanService.Models.Entities
{
    public class loanagreement
    {
        public Guid id { get; set; }
        public Guid loanid { get; set; }
        public string filepath { get; set; } = null!;
        public bool issigned { get; set; }
        public DateTime? signedat { get; set; }
        public DateTime createdat { get; set; }
    }
}
