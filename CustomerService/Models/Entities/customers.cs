using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models.Entities
{
    public class customers
    {
        [Key]
        public Guid id { get; set; }
        public Guid userid { get; set; }
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public DateTime? dateofbirth { get; set; }
        public DateTime createdat { get; set; }
        public DateTime? updatedat { get; set; }
    }
}
