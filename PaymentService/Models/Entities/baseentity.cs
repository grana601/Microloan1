using System;

namespace PaymentService.Models.Entities
{
    public abstract class baseentity<tid>
    {
        public tid id { get; set; } = default!;
        public bool isactive { get; set; }
        public string createdby { get; set; } = null!;
        public DateTime createddate { get; set; }
        public string? updatedby { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
