using System;

namespace LoanService.Models.Entities
{
    public class penaltyinterest
    {
        public Guid id { get; set; }
        public decimal dailyrate { get; set; }
        public int graceperioddays { get; set; }
        public DateTime effectivefrom { get; set; }
    }
}
