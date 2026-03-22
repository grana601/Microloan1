using System;

namespace PaymentService.Models.Entities
{
    public class paymenttransaction : baseentity<long>
    {
        public long loanid { get; set; }
        public string userid { get; set; } = null!;
        public decimal amount { get; set; }
        public string paymenttype { get; set; } = null!;
        public string status { get; set; } = null!;
        public DateTime transactiondate { get; set; }
        public string gatewayreference { get; set; } = null!;
    }
}
