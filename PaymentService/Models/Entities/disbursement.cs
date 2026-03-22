using System;

namespace PaymentService.Models.Entities
{
    public class disbursement : baseentity<long>
    {
        public long loanid { get; set; }
        public string userid { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime disbursedate { get; set; }
        public string transactionreference { get; set; } = null!;
        public string status { get; set; } = null!;
    }
}
