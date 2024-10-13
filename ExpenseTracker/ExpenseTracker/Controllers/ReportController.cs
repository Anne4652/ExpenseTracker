using ExpenseTracker.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                return BadRequest(new { message = "Start date cannot be later than end date." });
            }

            var fileContent = await _reportService.GenerateReportAsync(startDate, endDate);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExpenseReport.xlsx");
        }
    }
}
