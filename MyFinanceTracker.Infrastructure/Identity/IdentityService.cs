using Microsoft.AspNetCore.Identity;
using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Authentication.DTOs;

namespace MyFinanceTracker.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser 
            {  
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                PasswordHash = registerRequest.Password,
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
                await _userManager.AddToRoleAsync(user, "User");
            }

            return Result<string>.Success("User registered successfully.");
        }

        public async Task<Result<string>> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user is null)
            {
                return Result<string>.Failure("Invalid credentials.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginRequest.Email,
                loginRequest.Password, 
                isPersistent: false, 
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Result<string>.Failure("Invalid credentials.");
            }

            return Result<string>.Success(user.Id);
        }
    }
}
