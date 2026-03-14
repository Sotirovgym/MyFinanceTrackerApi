using FluentValidation;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IIdentityService identityService, ILogger<AuthController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <param name="registerValidator">Validator for the registration request (injected).</param>
        /// <returns>An HTTP response indicating the result of the registration.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registers a new user.",
            Description = "Creates a new user account with the provided registration details."
        )]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest? request,
            [FromServices] IValidator<RegisterRequest> registerValidator)
        {
            if (request is null)
            {
                _logger.LogWarning("Register called with null request body");
                return BadRequest("Request body is required.");
            }

            var validationResult = await registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Registration validation failed for {Email}", request.Email);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _identityService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Registration failed for {Email}: {Error}", request.Email, result.Error);
                return BadRequest(result.Error);
            }

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return Ok(result.Data);
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <param name="loginValidator">Validator for the login request (injected).</param>
        /// <returns>An HTTP response containing the authentication token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Authenticates a user.",
            Description = "Validates the user's credentials and returns a JWT bearer token if successful."
        )]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest? request,
            [FromServices] IValidator<LoginRequest> loginValidator)
        {
            if (request is null)
            {
                _logger.LogWarning("Login called with null request body");
                return BadRequest("Request body is required.");
            }

            var validationResult = await loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Login validation failed for {Email}", request.Email);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _identityService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Login failed for {Email}: invalid credentials", request.Email);
                return Unauthorized(result.Error);
            }

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);
            return Ok(result.Data);
        }
    }
}
