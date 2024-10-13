using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateReportAsync(DateTime? startDate, DateTime? endDate);
    }
}
