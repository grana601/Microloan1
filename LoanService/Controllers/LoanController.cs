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

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyLoan([FromBody] ApplyLoanRequestDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid request.");

        var success = await _loanService.ApplyLoanAsync(dto);
        if (success)
            return Ok(new { message = "Loan application submitted successfully." });
        
        return StatusCode(500, "An error occurred while processing the loan application.");
    }
}
