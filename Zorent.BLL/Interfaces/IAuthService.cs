using System;
using System.Collections.Generic;
using System.Text;
using Zorent.BLL.DTOs.Auth;

namespace Zorent.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);

        Task<object> GetUserByEmail(string email);
    }
}
