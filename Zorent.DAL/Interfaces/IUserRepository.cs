using System;
using System.Collections.Generic;
using System.Text;
using Zorent.Domain.Entities;

namespace Zorent.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> GetByEmail(string email);
    }
}
