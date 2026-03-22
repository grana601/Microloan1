using System;

namespace CustomerService.Models.Entities
{
    public class customerprofile : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string title { get; set; } = null!;
        public string firstname { get; set; } = null!;
        public string middlename { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public DateTime dob { get; set; }
        public string nationalinsurance { get; set; } = null!;
        public string maritalstatus { get; set; } = null!;
        public string employmentstatus { get; set; } = null!;
    }
}
