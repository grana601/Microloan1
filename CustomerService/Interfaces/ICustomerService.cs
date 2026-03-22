using CustomerService.Models.DTOs;

namespace CustomerService.Interfaces;

public interface ICustomerService
{
    Task<Guid> CreateCustomerAsync(CreateCustomerDto dto);
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
}
