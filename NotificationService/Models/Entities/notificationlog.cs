using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models.Entities
{
    public class notificationlog
    {
        [Key]
        public Guid id { get; set; }
        public Guid notificationid { get; set; }
        public string response { get; set; } = null!;
        public string status { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
