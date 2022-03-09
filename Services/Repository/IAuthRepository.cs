using RealTimeChat.Models;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Services.Repository
{
    interface IAuthRepository
    {
        Task<User> RegisterUser(RegisterViewModel model,bool isModelValid);
        Task<User> Login(LoginViewModel model,bool isModelValid);

    }
}
