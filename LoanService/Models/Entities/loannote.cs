namespace LoanService.Models.Entities
{
    public class loannote : baseentity<long>
    {
        public long loanid { get; set; }
        public string userid { get; set; } = null!;
        public string notetext { get; set; } = null!;
        public string notetype { get; set; } = null!;
    }
}
