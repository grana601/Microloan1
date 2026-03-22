using System;

namespace NotificationService.Models.Entities
{
    public class communication : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public long? loanid { get; set; }
        public string channel { get; set; } = null!;
        public string templatename { get; set; } = null!;
        public string content { get; set; } = null!;
        public string sentto { get; set; } = null!;
        public string status { get; set; } = null!;
        public DateTime? sentdate { get; set; }
    }
}
