using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound(new { message = "Expense not found." });
            }
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            await _expenseService.AddExpenseAsync(expenseDto);
            return CreatedAtAction(nameof(GetExpense), new { id = expenseDto.Id }, expenseDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense([FromBody] ExpenseDto expenseDto)
        {
            await _expenseService.UpdateExpenseAsync(expenseDto);
            return Ok(expenseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }
    }
}
