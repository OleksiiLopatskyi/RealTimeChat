using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Services.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private ChatAppContext _db;
        public AuthRepository(ChatAppContext context)
        {
            _db = context;
        }
        public async Task<User> Login(LoginViewModel model,bool isModelValid)
        {
            if (!isModelValid)
                return null;

           var user = await _db.Users.Include(i => i.AuthInfo).
                  FirstOrDefaultAsync(i =>
                  (i.AuthInfo.Email == model.Login ||
                  i.AuthInfo.Username == model.Login) &&
                  i.AuthInfo.Password == model.Password);

            return user;
        }

        public async Task<User> RegisterUser(RegisterViewModel model,bool isModelValid)
        {
            if (!isModelValid)
                return null;

            var createdUserAuth = new UserAuth()
            {
                Email = model.Email,
                Username = model.Username,
                Password = model.Password
            };
            var createdUser = new User()
            {
                AuthInfo = createdUserAuth,
                Username = createdUserAuth.Username
            };

            try
            {
                await _db.Users.AddAsync(createdUser);
                await _db.SaveChangesAsync();
                return createdUser;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
