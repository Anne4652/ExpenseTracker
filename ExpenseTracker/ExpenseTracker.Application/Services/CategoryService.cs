using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IValidator<Category> _categoryValidator;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IValidator<Category> categoryValidator,
                               ILogger<CategoryService> logger, IUserContextService userContextService, IMapper mapper,
                               IExpenseRepository expenseRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = categoryValidator;
            _logger = logger;
            _userContextService = userContextService;
            _mapper = mapper;
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Fetching all categories for user: {UserId}", userId);

            var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var userId = _userContextService.GetUserId();

            if (id <= 0)
                throw new ArgumentException("Invalid category ID.");

            _logger.LogInformation("Fetching category with ID: {Id} for user: {UserId}", id, userId);

            var category = await _categoryRepository.FindOneAsync(c => c.Id == id && c.UserId == userId);
            if (category == null)
                throw new ArgumentException("Category not found.");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            var result = _categoryValidator.Validate(category);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var userId = _userContextService.GetUserId();
            category.UserId = userId;

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            _logger.LogInformation("Added new category: {Name} by user: {UserId}", category.Name, userId);
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var userId = _userContextService.GetUserId();

            var existingCategory = await _categoryRepository.FindOneAsync(c => c.Id == categoryDto.Id && c.UserId == userId);
            if (existingCategory == null)
                throw new ArgumentException("Category not found or you do not have permission to update this category.");

            existingCategory.Name = categoryDto.Name;

            var result = _categoryValidator.Validate(existingCategory);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _categoryRepository.UpdateAsync(existingCategory);
            await _categoryRepository.SaveAsync();
            _logger.LogInformation("Updated category: {Name} for user: {UserId}", existingCategory.Name, userId);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var userId = _userContextService.GetUserId();

            if (id <= 0)
                throw new ArgumentException("Invalid category ID.");

            var category = await _categoryRepository.FindOneAsync(c => c.Id == id && c.UserId == userId);
            if (category == null)
                throw new ArgumentException("Category not found.");

            var hasExpenses = await _expenseRepository.FindAsync(e => e.CategoryId == id);
            if (hasExpenses.Any())
                throw new InvalidOperationException("Cannot delete category as it has associated expenses.");

            await _categoryRepository.DeleteAsync(category);
            await _categoryRepository.SaveAsync();
            _logger.LogInformation("Deleted category with ID: {Id} for user: {UserId}", id, userId);
        }
    }
}
