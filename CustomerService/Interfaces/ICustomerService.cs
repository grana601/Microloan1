using CustomerService.Models.DTOs;

namespace CustomerService.Interfaces;

public interface ICustomerService
{
    Task<Guid?> CreateCustomerAsync(CreateCustomerDto dto);
    Task<bool> CreateAddressAsync(CustomerAddressRequest request);
    Task<bool> CreateBankAsync(CustomerBankRequest request);
    Task<bool> CreateEmploymentAsync(CustomerEmploymentRequest request);
    Task<bool> CreateDebtAsync(CustomerDebtRequest request);
}
