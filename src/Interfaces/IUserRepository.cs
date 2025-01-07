using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.src.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByEmail(string email);
        Task<IdentityResult> CreateUser(AppUser user, string password);
        Task<bool> CheckPassword(AppUser user, string password);
        Task<AppUser?> GetUserById(string userId);
    }
}