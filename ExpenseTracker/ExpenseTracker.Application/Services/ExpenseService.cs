using AutoMapper;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IValidator<Expense> _expenseValidator;
        private readonly ILogger<ExpenseService> _logger;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IValidator<Expense> expenseValidator,
                              ILogger<ExpenseService> logger, IUserContextService userContextService, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _expenseValidator = expenseValidator;
            _logger = logger;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Fetching all expenses for user: {UserId}", userId);

            var expenses = await _expenseRepository.GetAllWithCategoryByUserIdAsync(userId); // Use new repository method
            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int id)
        {
            var userId = _userContextService.GetUserId();

            if (id <= 0)
                throw new ArgumentException("Invalid expense ID.");

            _logger.LogInformation("Fetching expense with ID: {Id} for user: {UserId}", id, userId);

            var expense = await _expenseRepository.GetByIdWithCategoryByUserIdAsync(id, userId); // Use new repository method

            if (expense == null)
                throw new ArgumentException("Expense not found.");

            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task AddExpenseAsync(ExpenseDto expenseDto)
        {
            var expense = _mapper.Map<Expense>(expenseDto);

            var result = _expenseValidator.Validate(expense);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var userId = _userContextService.GetUserId();
            expense.Category.UserId = userId;

            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveAsync();
            _logger.LogInformation("Added new expense: {Description} for user: {UserId}", expense.Description, userId);
        }

        public async Task UpdateExpenseAsync(ExpenseDto expenseDto)
        {
            var userId = _userContextService.GetUserId();

            var existingExpense = await _expenseRepository.GetByIdWithCategoryByUserIdAsync(expenseDto.Id, userId);
            if (existingExpense == null)
                throw new ArgumentException("Expense not found or you do not have permission to update this expense.");

            existingExpense.Amount = expenseDto.Amount;
            existingExpense.Description = expenseDto.Description;
            existingExpense.Date = expenseDto.Date;
            existingExpense.CategoryId = expenseDto.CategoryId;

            var result = _expenseValidator.Validate(existingExpense);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            _expenseRepository.UpdateAsync(existingExpense);
            await _expenseRepository.SaveAsync();
            _logger.LogInformation("Updated expense: {Description} for user: {UserId}", existingExpense.Description, userId);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var userId = _userContextService.GetUserId();

            if (id <= 0)
                throw new ArgumentException("Invalid expense ID.");

            var expense = await _expenseRepository.GetByIdWithCategoryByUserIdAsync(id, userId);
            if (expense == null)
                throw new ArgumentException("Expense not found.");

            await _expenseRepository.DeleteAsync(expense);
            await _expenseRepository.SaveAsync();
            _logger.LogInformation("Deleted expense with ID: {Id} for user: {UserId}", id, userId);
        }
    }
}
