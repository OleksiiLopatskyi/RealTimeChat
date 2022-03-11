using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using RealTimeChat.Services.Repository;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealTimeChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthRepository _authRepository;
        public AccountController(ChatAppContext shoppingContext)
        {
            _authRepository = new AuthRepository(shoppingContext);
        }
       
        [HttpGet]
        [Route("[controller]/[action]")]
        public IActionResult Login()
        {
            if(!User.Identity.IsAuthenticated) return View();

            return RedirectToAction("Index","direct");
        }
        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var foundUser = await _authRepository.Login(model,ModelState.IsValid);
            if (foundUser==null)
                return BadRequest("Login or password is incorrect");

            await Authenticate(foundUser.AuthInfo);
            return RedirectToAction("Index","direct");
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public IActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
                return View();

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var registeredUser = await _authRepository.RegisterUser(model,ModelState.IsValid);
            if (registeredUser == null)
                return BadRequest(ModelState);

            await Authenticate(registeredUser.AuthInfo);
            return RedirectToAction("Index","Home");
        }
        private async Task Authenticate(UserAuth userAuth)
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAuth.Username),
                    new Claim("Id",userAuth.UserId.ToString())
                };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                                                   ClaimsIdentity.DefaultNameClaimType,
                                                   ClaimsIdentity.DefaultRoleClaimType
                                                   );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
