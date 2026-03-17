using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Categories.DTOs;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Application.Features.Categories
{
    internal sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result<CategoryResponse>.Failure("Category not found.");
            }
            return Result<CategoryResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result<IReadOnlyList<CategoryResponse>>> GetAllAsync(string userId, bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var list = await _categoryRepository.GetAllAsync(userId, includeInactive, cancellationToken);
            return Result<IReadOnlyList<CategoryResponse>>.Success(list.Select(MapToResponse).ToList());
        }

        public async Task<Result<CategoryResponse>> CreateAsync(string userId, CreateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var entity = new Category
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Type = request.Type,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _categoryRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<CategoryResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result<CategoryResponse>> UpdateAsync(string userId, Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result<CategoryResponse>.Failure("Category not found.");
            }
            entity.Name = request.Name;
            entity.Type = request.Type;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<CategoryResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _categoryRepository.GetByIdAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result.Failure("Category not found.");
            }
            await _categoryRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private static CategoryResponse MapToResponse(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Type = c.Type,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };
    }
}
