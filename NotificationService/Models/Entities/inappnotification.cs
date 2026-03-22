using System;

namespace NotificationService.Models.Entities
{
    public class inappnotification : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string title { get; set; } = null!;
        public string message { get; set; } = null!;
        public bool isread { get; set; }
        public DateTime? readdate { get; set; }
    }
}
