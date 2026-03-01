using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Authentication.DTOs;
using MyFinanceTracker.Core.Constants;
using MyFinanceTracker.Infrastructure.Options;

namespace MyFinanceTracker.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser 
            {  
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                EnableNotifications = registerRequest.EnableNotifications
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure($"Registration failed: {errors}");
            }

            if (result.Succeeded)
            {
                // Assign default role
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return Result<string>.Success("User registered successfully.");
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null)
            {
                return Result<LoginResponse>.Failure("Invalid credentials.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                loginRequest.Email,
                loginRequest.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return Result<LoginResponse>.Failure("Invalid credentials.");
            }

            var accessToken = await GenerateAccessTokenAsync(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            return Result<LoginResponse>.Success(new LoginResponse
            {
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            });
        }

        private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
