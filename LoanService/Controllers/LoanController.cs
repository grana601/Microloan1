using LoanService.Interfaces;
using LoanService.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Controllers;

[ApiController]
[Route("api/loans")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDto dto)
    {
        var id = await _loanService.CreateLoanAsync(dto);
        return CreatedAtAction(nameof(GetLoan), new { id }, new { id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoan(Guid id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        if (loan == null) return NotFound();

        return Ok(loan);
    }
}
