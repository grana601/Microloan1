using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models.Entities
{
    public class customerbank
    {
        [Key]
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public string accountholdername { get; set; } = null!;
        public string bankname { get; set; } = null!;
        public string accountnumber { get; set; } = null!;
        public string sortcode { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
