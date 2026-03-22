using CustomerService.Interfaces;
using CustomerService.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var id = await _customerService.CreateCustomerAsync(dto);
        return CreatedAtAction(nameof(GetCustomer), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();

        return Ok(customer);
    }
}
