namespace LoanService.Models.Entities
{
    public class loanproductconfig : baseentity<long>
    {
        public decimal monthlyinterestrate { get; set; }
        public decimal penaltydailyrate { get; set; }
        public decimal maxloanamount { get; set; }
        public decimal minloanamount { get; set; }
    }
}
