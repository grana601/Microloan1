namespace CustomerService.Models.Entities
{
    public class customerdebt : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string debttype { get; set; } = null!;
        public decimal outstandingamount { get; set; }
        public decimal monthlypayment { get; set; }
        public bool inbankruptcy { get; set; }
        public bool hasccj { get; set; }
    }
}
