using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Features.Authentication.DTOs;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        public Task<Result<string>> RegisterAsync(RegisterRequest registerRequest);
        public Task<Result<string>> LoginAsync(LoginRequest loginRequest);
    }
}
