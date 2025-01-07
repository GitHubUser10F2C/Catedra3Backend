using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.Interfaces;
using backend.src.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.src.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckPassword(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<AppUser?> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser?> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}