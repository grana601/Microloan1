using System.Net.Http.Json;
using System.Text.Json;

namespace LoanService.Services.External;

public interface IIdentityServiceClient
{
    Task<bool> CheckUserAsync(string username);
    Task<string?> RegisterUserAsync(RegisterUserRequest request);
    Task<bool> AssignRoleAsync(string username, string role);
}

public class IdentityServiceClient : IIdentityServiceClient
{
    private readonly HttpClient _httpClient;

    public IdentityServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckUserAsync(string username)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/check-user", new { Username = username });
        return response.IsSuccessStatusCode; // Assuming 200 OK means user exists
    }

    public async Task<string?> RegisterUserAsync(RegisterUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("userId", out var userIdElement))
            {
                return userIdElement.GetString();
            }
            return "Registered"; // generic success if no ID is returned
        }
        return null;
    }

    public async Task<bool> AssignRoleAsync(string username, string role)
    {
        // Add helper for assigning role if needed, or assume it's part of register
        var response = await _httpClient.PostAsJsonAsync("/api/auth/assign-role", new { Username = username, Role = role });
        return response.IsSuccessStatusCode;
    }
}

public class RegisterUserRequest
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
}
