using ExpenseTracker.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllWithCategoryByUserIdAsync(string userId);
        Task<Expense> GetByIdWithCategoryByUserIdAsync(int id, string userId);
    }
}
