namespace CustomerService.Models.Entities
{
    public class customerdocument : baseentity<long>
    {
        public string userid { get; set; } = null!;
        public string documenttype { get; set; } = null!;
        public string filepath { get; set; } = null!;
        public string verificationstatus { get; set; } = null!;
    }
}
