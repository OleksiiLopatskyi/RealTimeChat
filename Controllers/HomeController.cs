using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using RealTimeChat.Services.Repository;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Controllers
{
   
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        public HomeController(ILogger<HomeController> logger,ChatAppContext context)
        {
            _logger = logger;
            _repository = new Repository(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value);
            var users = await _repository.GetAllUsers();
            return Ok(users.Where(i => i.Id != GetUserId()));
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateChat(int receiverId)
        {
            var getChat = await _repository.CreateChat(GetUserId(),receiverId);

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private int GetUserId() => Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value);

    }
}
