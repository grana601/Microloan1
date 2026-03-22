namespace CustomerService.Models.Entities
{
    public class customerincome : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string incomerange { get; set; } = null!;
        public string primarysource { get; set; } = null!;
        public decimal totalmonthlyincome { get; set; }
        public int dependentcount { get; set; }
    }
}
