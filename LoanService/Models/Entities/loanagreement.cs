using System;

namespace LoanService.Models.Entities
{
    public class loanagreement : baseentity<long>
    {
        public long loanid { get; set; }
        public string userid { get; set; } = null!;
        public string agreementtext { get; set; } = null!;
        public string documentpath { get; set; } = null!;
        public bool issigned { get; set; }
        public DateTime? signeddate { get; set; }
        public string envelopeid { get; set; } = null!;
    }
}
