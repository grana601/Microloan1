namespace CustomerService.Models.Entities
{
    public class customeraddress : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string line1 { get; set; } = null!;
        public string line2 { get; set; } = null!;
        public string city { get; set; } = null!;
        public string state { get; set; } = null!;
        public string country { get; set; } = null!;
        public string postcode { get; set; } = null!;
        public string addresstype { get; set; } = null!;
    }
}
