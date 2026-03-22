namespace CustomerService.Models.Entities
{
    public class customerbank : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string accountholder { get; set; } = null!;
        public string bankname { get; set; } = null!;
        public string sortcode { get; set; } = null!;
        public string accountnumber { get; set; } = null!;
        public bool isverified { get; set; }
    }
}
