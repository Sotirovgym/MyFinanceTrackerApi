using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Transactions.DTOs;
using MyFinanceTracker.Application.Features.Transactions.Filters;

namespace MyFinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(
            ITransactionService transactionService,
            ICurrentUserService currentUser,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _currentUser = currentUser;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetByIdAsync(_currentUser.UserId, id, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Transaction not found. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            [FromQuery] TransactionFilterRequest filter,
            [FromServices] IValidator<TransactionFilterRequest> filterValidator,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await filterValidator.ValidateAsync(filter, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Get transactions validation failed. UserId: {UserId}, AccountId: {AccountId}, CategoryId: {CategoryId}, From: {From}, To: {To}", 
                    _currentUser.UserId, filter.AccountId, filter.CategoryId, filter.From, filter.To);

                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _transactionService.GetAllAsync(_currentUser.UserId, filter, cancellationToken);
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateTransactionRequest? request,
            [FromServices] IValidator<CreateTransactionRequest> createValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Create transaction called with null request body. UserId: {UserId}", _currentUser.UserId);
                return BadRequest("Request body is required.");
            }

            var validationResult = await createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Create transaction validation failed. UserId: {UserId}, AccountId: {AccountId}, CategoryId: {CategoryId}", 
                    _currentUser.UserId, request.AccountId, request.CategoryId);

                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _transactionService.CreateAsync(_currentUser.UserId, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Create transaction failed. UserId: {UserId}, AccountId: {AccountId}, CategoryId: {CategoryId}, Error: {Error}", 
                    _currentUser.UserId, request.AccountId, request.CategoryId, result.Error);

                return BadRequest(result.Error);
            }

            _logger.LogInformation("Transaction created. UserId: {UserId}, TransactionId: {TransactionId}, AccountId: {AccountId}, Type: {Type}, Amount: {Amount}", _currentUser.UserId, result.Data!.Id, result.Data.AccountId, result.Data.Type, result.Data.Amount);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateTransactionRequest? request,
            [FromServices] IValidator<UpdateTransactionRequest> updateValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Update transaction called with null request body. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
                return BadRequest("Request body is required.");
            }

            var validationResult = await updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Update transaction validation failed. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _transactionService.UpdateAsync(_currentUser.UserId, id, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Transaction not found for update. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Transaction updated. UserId: {UserId}, TransactionId: {TransactionId}, AccountId: {AccountId}, Type: {Type}, Amount: {Amount}", 
                _currentUser.UserId, id, result.Data!.AccountId, result.Data.Type, result.Data.Amount);

            return Ok(result.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _transactionService.DeleteAsync(_currentUser.UserId, id, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Transaction not found for delete. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Transaction deleted. UserId: {UserId}, TransactionId: {TransactionId}", _currentUser.UserId, id);
            return NoContent();
        }
    }
}
