using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Authentication.DTOs;

namespace MyFinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _identityService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Data);
        }
    }
}
