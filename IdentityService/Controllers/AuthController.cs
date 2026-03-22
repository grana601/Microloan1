using IdentityService.Interfaces;
using IdentityService.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IIdentityAppService _identityService;

    public AuthController(IIdentityAppService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var id = await _identityService.RegisterUserAsync(dto);
        return Ok(new { Id = id });
    }
}
