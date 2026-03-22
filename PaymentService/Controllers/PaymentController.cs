using Microsoft.AspNetCore.Mvc;
using PaymentService.Interfaces;
using PaymentService.Models.DTOs;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
    {
        var id = await _paymentService.CreatePaymentAsync(dto);
        return CreatedAtAction(nameof(GetPayment), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayment(Guid id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null) return NotFound();

        return Ok(payment);
    }
}
