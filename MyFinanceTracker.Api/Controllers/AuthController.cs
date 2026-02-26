using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Authentication.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinanceTracker.Api.Controllers
{
    /// <summary>
    /// Handles user authentication.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="identityService">The identity service for user authentication and registration.</param>
        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>An HTTP response indicating the result of the registration.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registers a new user.",
            Description = "Creates a new user account with the provided registration details."
        )]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>An HTTP response containing the authentication token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Authenticates a user.",
            Description = "Validates the user's credentials and returns an authentication token if successful."
        )]
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
