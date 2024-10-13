using ExpenseTracker.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
        Task<NewUserDto> RegisterAsync(RegisterDto registerDto);
    }

}
