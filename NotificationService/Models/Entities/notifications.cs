using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models.Entities
{
    public class notifications
    {
        [Key]
        public Guid id { get; set; }
        public Guid userid { get; set; }
        public string message { get; set; } = null!;
        public string type { get; set; } = null!;
        public string status { get; set; } = null!;
        public DateTime createdat { get; set; }
    }
}
