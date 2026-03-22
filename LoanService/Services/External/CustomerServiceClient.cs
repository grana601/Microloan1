using System.Net.Http.Json;
using System.Text.Json;
using LoanService.Models.DTOs;

namespace LoanService.Services.External;

public interface ICustomerServiceClient
{
    Task<Guid?> CreateCustomerAsync(CreateCustomerRequest request);
    Task<bool> CreateAddressAsync(Guid customerId, AddressDto address);
    Task<bool> CreateBankAsync(Guid customerId, BankDetailsDto bank);
    Task<bool> CreateEmploymentAsync(Guid customerId, EmploymentDetailsDto employment);
    Task<bool> CreateDebtAsync(Guid customerId, DebtInfoDto debt);
}

public class CustomerServiceClient : ICustomerServiceClient
{
    private readonly HttpClient _httpClient;

    public CustomerServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid?> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/customers", request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("id", out var idElement))
            {
                if (Guid.TryParse(idElement.GetString(), out var id))
                    return id;
            }
            return Guid.NewGuid(); // Fallback
        }
        return null; // failed
    }

    public async Task<bool> CreateAddressAsync(Guid customerId, AddressDto address)
    {
        var payload = new { CustomerId = customerId, Address = address };
        var response = await _httpClient.PostAsJsonAsync("/api/customeraddress", payload);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateBankAsync(Guid customerId, BankDetailsDto bank)
    {
        var payload = new { CustomerId = customerId, BankDetails = bank };
        var response = await _httpClient.PostAsJsonAsync("/api/customerbank", payload);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateEmploymentAsync(Guid customerId, EmploymentDetailsDto employment)
    {
        var payload = new { CustomerId = customerId, EmploymentDetails = employment };
        var response = await _httpClient.PostAsJsonAsync("/api/customeremployment", payload);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateDebtAsync(Guid customerId, DebtInfoDto debt)
    {
        var payload = new { CustomerId = customerId, DebtInfo = debt };
        var response = await _httpClient.PostAsJsonAsync("/api/customerdebt", payload);
        return response.IsSuccessStatusCode;
    }
}

public class CreateCustomerRequest
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
}
