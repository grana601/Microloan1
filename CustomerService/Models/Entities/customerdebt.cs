using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models.Entities
{
    public class customerdebt
    {
        [Key]
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public string debttype { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime createdat { get; set; }
    }
}
