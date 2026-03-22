using System;

namespace IdentityService.Models.Entities
{
    public class refreshtoken
    {
        public Guid id { get; set; }
        public string userid { get; set; } = null!;
        public string token { get; set; } = null!;
        public DateTime expirydate { get; set; }
        public bool isrevoked { get; set; }
        public DateTime createdat { get; set; }
    }
}
