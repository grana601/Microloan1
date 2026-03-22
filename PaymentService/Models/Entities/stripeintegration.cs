namespace PaymentService.Models.Entities
{
    public class stripeintegration : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string stripecustomerid { get; set; } = null!;
        public string stripepaymentmethodid { get; set; } = null!;
        public string defaultmandateid { get; set; } = null!;
        public bool ismandateactive { get; set; }
    }
}
