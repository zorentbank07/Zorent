using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Zorent.BLL.DTOs.Auth;
using Zorent.BLL.Interfaces;
using Zorent.BLL.Validators;
using Zorent.DAL.Data;
using Zorent.DAL.Interfaces;
using Zorent.Domain.Entities;

namespace Zorent.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;

        public AuthService(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }



        public async Task<string> Register(RegisterDto dto)
        {
            if (!PasswordValidator.IsValid(dto.Password))
                return "Password must be strong";

            var exists = await _context.Users
                .AnyAsync(x =>
                    x.Email == dto.Email ||
                    x.Username == dto.Username);

            if (exists)
                return "User already exists";

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                DOB = dto.DOB,
                Address = dto.Address,
                Username = dto.Username,

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),

                FailedAttempts = 0,
                IsLocked = false
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return "Registration successful";
        }


        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Username == dto.UserName);

            if (user == null)
                return "Invalid username or password";

            if (user.IsLocked)
                return "Account is locked";

            bool isPasswordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!isPasswordCorrect)
            {
                user.FailedAttempts++;

                if (user.FailedAttempts >= 3)
                {
                    user.IsLocked = true;
                }

                await _context.SaveChangesAsync();

                return "Invalid username or password";
            }

            user.FailedAttempts = 0;

            await _context.SaveChangesAsync();

            return "Login successful";
        }

        public async Task<object> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                return "User not found";

            return new
            {
                user.FullName,
                user.Email,
                user.Phone,
                user.Address,
                user.Username
            };
        }
    }
}