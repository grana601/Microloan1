using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Data;
using IdentityService.Interfaces;
using IdentityService.Models.DTOs;
using IdentityService.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.username,
                Email = dto.email
            };

            var result = await _userManager.CreateAsync(user, dto.password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }

            // Provide default role
            var roleResult = await _userManager.AddToRoleAsync(user, "customer");

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.username);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid username or password");
            }

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            // Extract Principal from expired token
            var principal = GetPrincipalFromExpiredToken(dto.accesstoken);
            if (principal == null)
            {
                throw new Exception("Invalid access token");
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue("userid");
            if (userId == null)
            {
                throw new Exception("Invalid access token payload");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Validate refresh token from DB
            var storedToken = await _dbContext.refreshtokens
                .FirstOrDefaultAsync(rt => rt.token == dto.refreshtoken && rt.userid == user.Id);

            if (storedToken == null || storedToken.isrevoked || storedToken.expirydate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }

            // Revoke old token
            storedToken.isrevoked = true;
            _dbContext.refreshtokens.Update(storedToken);
            await _dbContext.SaveChangesAsync();

            // Generate new token pair
            return await GenerateAuthResponseAsync(user);
        }

        public async Task<bool> LogoutAsync(string userid)
        {
            var activeTokens = await _dbContext.refreshtokens
                .Where(rt => rt.userid == userid && !rt.isrevoked)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.isrevoked = true;
            }

            _dbContext.refreshtokens.UpdateRange(activeTokens);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<bool> AssignRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }
            return true;
        }

        private async Task<AuthResponseDto> GenerateAuthResponseAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var jwtConfig = _configuration.GetSection("jwt");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("userid", user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim("username", user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("roles", role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiryMinutes = int.Parse(jwtConfig["accesstokenexpiryminutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtConfig["issuer"],
                audience: jwtConfig["audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshTokenString = GenerateRefreshTokenString();

            var refreshToken = new refreshtoken
            {
                id = Guid.NewGuid(),
                userid = user.Id,
                token = refreshTokenString,
                createdat = DateTime.UtcNow,
                expirydate = DateTime.UtcNow.AddDays(int.Parse(jwtConfig["refreshtokenexpirydays"]!)),
                isrevoked = false
            };

            _dbContext.refreshtokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResponseDto
            {
                accesstoken = jwt,
                refreshtoken = refreshTokenString
            };
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var jwtConfig = _configuration.GetSection("jwt");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = jwtConfig["audience"],
                ValidateIssuer = true,
                ValidIssuer = jwtConfig["issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["key"]!)),
                ValidateLifetime = false // Here we intentionally don't validate lifetime because the token is expected to be expired
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}
