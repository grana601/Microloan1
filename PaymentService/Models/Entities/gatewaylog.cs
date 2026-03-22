using System;

namespace PaymentService.Models.Entities
{
    public class gatewaylog : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public long? loanid { get; set; }
        public string eventtype { get; set; } = null!;
        public string rawpayload { get; set; } = null!;
        public string status { get; set; } = null!;
        public DateTime timestamp { get; set; }
    }
}
