using System;

namespace CustomerService.Models.Entities
{
    public class customeraddress
    {
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public string line1 { get; set; } = null!;
        public string line2 { get; set; } = null!;
        public string city { get; set; } = null!;
        public string state { get; set; } = null!;
        public string country { get; set; } = null!;
        public string postcode { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
