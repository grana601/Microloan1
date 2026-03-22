using System;

namespace CustomerService.Models.Entities
{
    public class customerdebt
    {
        public Guid id { get; set; }
        public Guid customerid { get; set; }
        public string debttype { get; set; } = null!;
        public decimal amount { get; set; }
        public DateTime createdat { get; set; }
    }
}
