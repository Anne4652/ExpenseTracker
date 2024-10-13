using ExpenseTracker.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync();
        Task<ExpenseDto> GetExpenseByIdAsync(int id);
        Task AddExpenseAsync(ExpenseDto expenseDto);
        Task UpdateExpenseAsync(ExpenseDto expenseDto);
        Task DeleteExpenseAsync(int id);
    }
}
