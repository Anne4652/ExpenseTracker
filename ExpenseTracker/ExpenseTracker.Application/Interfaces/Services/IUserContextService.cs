using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IUserContextService
    {
        string GetUserId();
        ClaimsPrincipal GetUserClaimsPrincipal();
    }
}
