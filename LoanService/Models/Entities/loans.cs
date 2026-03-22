using System;
using System.ComponentModel.DataAnnotations;

namespace LoanService.Models.Entities
{
    public class loans
    {
        [Key]
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public decimal amount { get; set; }
        public int termmonths { get; set; }
        public decimal interestrate { get; set; }
        public string status { get; set; } = null!;
        public string purpose { get; set; } = null!;
        public DateTime createdat { get; set; }
        public DateTime? updatedat { get; set; }
    }
}
