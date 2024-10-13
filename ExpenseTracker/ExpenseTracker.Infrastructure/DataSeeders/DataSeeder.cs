using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.DataSeeders
{

    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DataSeeder(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var defaultUser = await _userManager.FindByEmailAsync("user@example.com");
            if (defaultUser == null)
            {
                defaultUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com" };
                await _userManager.CreateAsync(defaultUser, "Password123!");
            }

            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Food", UserId = defaultUser.Id },
                new Category { Name = "Utilities", UserId = defaultUser.Id },
                new Category { Name = "Entertainment", UserId = defaultUser.Id },
                new Category { Name = "Travel", UserId = defaultUser.Id }
            };

                _context.Categories.AddRange(categories);
                await _context.SaveChangesAsync();
            }

            if (!_context.Expenses.Any())
            {
                var expenses = new List<Expense>
            {
                new Expense { Amount = 50.00M, Description = "Grocery shopping", Date = DateTime.UtcNow, CategoryId = 1 },
                new Expense { Amount = 120.00M, Description = "Electricity bill", Date = DateTime.UtcNow, CategoryId = 2 },
                new Expense { Amount = 30.00M, Description = "Movie tickets", Date = DateTime.UtcNow, CategoryId = 3 },
                new Expense { Amount = 200.00M, Description = "Weekend getaway", Date = DateTime.UtcNow, CategoryId = 4 }
            };

                _context.Expenses.AddRange(expenses);
                await _context.SaveChangesAsync();
            }
        }
    }
}
