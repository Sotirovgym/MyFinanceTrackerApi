using System.Security.Claims;
using MyFinanceTracker.Application.Common.Interfaces;

namespace MyFinanceTracker.Api.Common.Services
{
    /// <summary>
    /// Resolves the current user from the HTTP request's authenticated principal.
    /// </summary>
    internal sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}
