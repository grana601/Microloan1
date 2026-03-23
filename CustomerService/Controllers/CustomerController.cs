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
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto request)
    {
        var customerId = await _customerService.CreateCustomerAsync(request);
        if (customerId.HasValue)
            return Ok(new { id = customerId.Value });
        return BadRequest("Failed to create customer.");
    }

    [HttpPost("/api/customeraddress")]
    public async Task<IActionResult> CreateAddress([FromBody] CustomerAddressRequest request)
    {
        var result = await _customerService.CreateAddressAsync(request);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpPost("/api/customerbank")]
    public async Task<IActionResult> CreateBank([FromBody] CustomerBankRequest request)
    {
        var result = await _customerService.CreateBankAsync(request);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpPost("/api/customeremployment")]
    public async Task<IActionResult> CreateEmployment([FromBody] CustomerEmploymentRequest request)
    {
        var result = await _customerService.CreateEmploymentAsync(request);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpPost("/api/customerdebt")]
    public async Task<IActionResult> CreateDebt([FromBody] CustomerDebtRequest request)
    {
        var result = await _customerService.CreateDebtAsync(request);
        if (result) return Ok();
        return BadRequest();
    }
}
