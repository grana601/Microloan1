using System.Security.Claims;
using System.Threading.Tasks;
using IdentityService.Interfaces;
using IdentityService.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("userid");
            if (userId == null) return Unauthorized();

            await _authService.LogoutAsync(userId);
            return Ok(new { Message = "Successfully logged out" });
        }
        [HttpPost("check-user")]
        public async Task<IActionResult> CheckUser([FromBody] CheckUserRequest request)
        {
            try
            {
                var exists = await _authService.CheckUserAsync(request.Username);
                if (exists) return Ok(new { Exists = true });
                return NotFound(new { Exists = false });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            try
            {
                var success = await _authService.AssignRoleAsync(request.Username, request.Role);
                if (success) return Ok(new { Message = "Role assigned successfully." });
                return BadRequest(new { Message = "Failed to assign role or user not found." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    public class CheckUserRequest
    {
        public string Username { get; set; } = null!;
    }

    public class AssignRoleRequest
    {
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}

