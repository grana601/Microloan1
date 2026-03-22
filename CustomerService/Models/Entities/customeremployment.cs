using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models.Entities
{
    public class customeremployment
    {
        [Key]
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public string employmentstatus { get; set; } = null!;
        public string incomesource { get; set; } = null!;
        public decimal monthlyincome { get; set; }
        public DateTime createdat { get; set; }
    }
}
