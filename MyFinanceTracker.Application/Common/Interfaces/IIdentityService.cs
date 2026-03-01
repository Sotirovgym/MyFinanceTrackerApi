using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Features.Authentication.DTOs;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<string>> RegisterAsync(RegisterRequest registerRequest);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest);
    }
}
