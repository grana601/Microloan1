namespace CustomerService.Models.DTOs;

public class CreateCustomerDto
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
}
