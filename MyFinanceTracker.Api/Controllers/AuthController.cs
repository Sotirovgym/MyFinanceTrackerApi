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
        private readonly IValidator<RegisterRequest> _registerValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="identityService">The identity service for user authentication and registration.</param>
        /// <param name="registerValidator">The validator for registration requests.</param>
        /// <param name="loginValidator">The validator for login requests.</param>
        /// <param name="logger">The logger for the controller.</param>
        public AuthController(
            IIdentityService identityService,
            IValidator<RegisterRequest> registerValidator,
            IValidator<LoginRequest> loginValidator,
            ILogger<AuthController> logger)
        {
            _identityService = identityService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>An HTTP response indicating the result of the registration.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registers a new user.",
            Description = "Creates a new user account with the provided registration details."
        )]
        public async Task<IActionResult> Register([FromBody] RegisterRequest? request)
        {
            if (request is null)
            {
                _logger.LogWarning("Register called with null request body");
                return BadRequest("Request body is required.");
            }

            var validationResult = await _registerValidator.ValidateAsync(request);
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
        /// <returns>An HTTP response containing the authentication token.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Authenticates a user.",
            Description = "Validates the user's credentials and returns a JWT bearer token if successful."
        )]
        public async Task<IActionResult> Login([FromBody] LoginRequest? request)
        {
            if (request is null)
            {
                _logger.LogWarning("Login called with null request body");
                return BadRequest("Request body is required.");
            }

            var validationResult = await _loginValidator.ValidateAsync(request);
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
