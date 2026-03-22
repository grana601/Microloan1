using System;

namespace PaymentService.Models.Entities
{
    public class repaymentschedule : baseentity<long>
    {
        public long loanid { get; set; }
        public string userid { get; set; } = null!;
        public DateTime duedate { get; set; }
        public decimal amountdue { get; set; }
        public decimal principaldue { get; set; }
        public decimal interestdue { get; set; }
        public decimal amountpaid { get; set; }
        public bool ispaid { get; set; }
        public DateTime? paiddate { get; set; }
    }
}
