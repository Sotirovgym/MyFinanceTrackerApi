using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Categories.DTOs;

namespace MyFinanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ICurrentUserService currentUser,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _currentUser = currentUser;
            _logger = logger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetByIdAsync(_currentUser.UserId, id, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Category not found. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var result = await _categoryService.GetAllAsync(_currentUser.UserId, includeInactive, cancellationToken);
            return Ok(result.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryRequest? request,
            [FromServices] IValidator<CreateCategoryRequest> createValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Create category called with null request body. UserId: {UserId}", _currentUser.UserId);
                return BadRequest("Request body is required.");
            }

            var validationResult = await createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Create category validation failed. UserId: {UserId}, Name: {Name}", _currentUser.UserId, request.Name);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _categoryService.CreateAsync(_currentUser.UserId, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Create category failed. UserId: {UserId}, Name: {Name}, Error: {Error}", _currentUser.UserId, request.Name, result.Error);
                return BadRequest(result.Error);
            }

            _logger.LogInformation("Category created. UserId: {UserId}, CategoryId: {CategoryId}, Name: {Name}", _currentUser.UserId, result.Data!.Id, result.Data.Name);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateCategoryRequest? request,
            [FromServices] IValidator<UpdateCategoryRequest> updateValidator,
            CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                _logger.LogWarning("Update category called with null request body. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
                return BadRequest("Request body is required.");
            }

            var validationResult = await updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Update category validation failed. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var result = await _categoryService.UpdateAsync(_currentUser.UserId, id, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Category not found for update. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Category updated. UserId: {UserId}, CategoryId: {CategoryId}, Name: {Name}", _currentUser.UserId, id, result.Data!.Name);
            return Ok(result.Data);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _categoryService.DeleteAsync(_currentUser.UserId, id, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Category not found for delete. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
                return NotFound(result.Error);
            }

            _logger.LogInformation("Category deleted. UserId: {UserId}, CategoryId: {CategoryId}", _currentUser.UserId, id);
            return NoContent();
        }
    }
}
