using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportService> _logger;
        private readonly IUserContextService _userContextService;

        public ReportService(ApplicationDbContext context, ILogger<ReportService> logger, IUserContextService userContextService)
        {
            _context = context;
            _logger = logger;
            _userContextService = userContextService;
        }

        public async Task<byte[]> GenerateReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var userId = _userContextService.GetUserId();
            _logger.LogInformation("Generating report for user: {UserId} from {StartDate} to {EndDate}", userId, startDate, endDate);

            var expensesQuery = _context.Expenses.Include(e => e.Category)
                                                 .Where(e => e.Category.UserId == userId)
                                                 .AsQueryable();

            if (startDate.HasValue)
                expensesQuery = expensesQuery.Where(e => e.Date >= startDate.Value);

            if (endDate.HasValue)
                expensesQuery = expensesQuery.Where(e => e.Date <= endDate.Value);

            var expenses = await expensesQuery.ToListAsync();
            var groupedExpenses = expenses.GroupBy(e => e.Category.Name).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Category";
                worksheet.Cells[1, 3].Value = "Description";
                worksheet.Cells[1, 4].Value = "Amount";
                worksheet.Cells[1, 5].Value = "Date";

                int row = 2;

                foreach (var categoryGroup in groupedExpenses)
                {
                    foreach (var expense in categoryGroup)
                    {
                        worksheet.Cells[row, 1].Value = expense.Id;
                        worksheet.Cells[row, 2].Value = expense.Category.Name;
                        worksheet.Cells[row, 3].Value = expense.Description;
                        worksheet.Cells[row, 4].Value = expense.Amount;
                        worksheet.Cells[row, 5].Value = expense.Date.ToString("yyyy-MM-dd");
                        row++;
                    }
                }

                _logger.LogInformation("Report generated with {RowCount} rows for user: {UserId}.", row - 2, userId);

                return package.GetAsByteArray();
            }
        }
    }
}
