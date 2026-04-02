using LoanService.Interfaces;
using LoanService.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        return Ok(new { message = "success" });
    }
    [Authorize]
    [HttpGet("Test1")]
    public async Task<IActionResult> Test1()
    {
        var data = _loanService.GetLoanListAsync();
        return Ok(new
        {
            message = "success",
            data
        });
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
