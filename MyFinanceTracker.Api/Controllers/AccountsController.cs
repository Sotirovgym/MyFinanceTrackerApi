using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Accounts.DTOs;

namespace MyFinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            IAccountService accountService,
            ICurrentUserService currentUser,
            ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _currentUser = currentUser;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _accountService.GetByIdAsync(_currentUser.UserId, id, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Account not found. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<AccountResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var result = await _accountService.GetAllAsync(_currentUser.UserId, includeInactive, cancellationToken);
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateAccountRequest? request,
            [FromServices] IValidator<CreateAccountRequest> createValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Create account called with null request body. UserId: {UserId}", _currentUser.UserId);
                return BadRequest("Request body is required.");
            }

            var validationResult = await createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Create account validation failed. UserId: {UserId}, Name: {Name}", _currentUser.UserId, request.Name);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _accountService.CreateAsync(_currentUser.UserId, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Create account failed. UserId: {UserId}, Name: {Name}, Error: {Error}", _currentUser.UserId, request.Name, result.Error);
                return BadRequest(result.Error);
            }

            _logger.LogInformation("Account created. UserId: {UserId}, AccountId: {AccountId}, Name: {Name}", _currentUser.UserId, result.Data!.Id, result.Data.Name);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateAccountRequest? request,
            [FromServices] IValidator<UpdateAccountRequest> updateValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Update account called with null request body. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
                return BadRequest("Request body is required.");
            }

            var validationResult = await updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Update account validation failed. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _accountService.UpdateAsync(_currentUser.UserId, id, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Account not found for update. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Account updated. UserId: {UserId}, AccountId: {AccountId}, Name: {Name}", _currentUser.UserId, id, result.Data!.Name);
            return Ok(result.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _accountService.DeleteAsync(_currentUser.UserId, id, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Account not found for delete. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Account deleted. UserId: {UserId}, AccountId: {AccountId}", _currentUser.UserId, id);
            return NoContent();
        }
    }
}
