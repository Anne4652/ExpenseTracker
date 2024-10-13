using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllWithCategoryByUserIdAsync(string userId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.Category.UserId == userId)
                .ToListAsync();
        }

        public async Task<Expense> GetByIdWithCategoryByUserIdAsync(int id, string userId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id && e.Category.UserId == userId);
        }
    }
}
