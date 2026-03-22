namespace IdentityService.Models.Entities
{
    public class menu : baseentity<long>
    {
        public long? parentid { get; set; }
        public string title { get; set; } = null!;
        public string url { get; set; } = null!;
        public string icon { get; set; } = null!;
        public bool isviewonly { get; set; }
    }
}
