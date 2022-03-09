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
        private ChatAppContext _db;
        private readonly IRepository _repository;
        public HomeController(ILogger<HomeController> logger,ChatAppContext context,IRepository repository)
        {
            _logger = logger;
            _db = context;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value);
            return View(await _repository.GetUserChats(userId));
        }
        [HttpGet]
        public async Task<IActionResult> GetChat(int chatId)
        {
            return Ok(await _repository.GetChatById(chatId));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageViewModel messageViewModel)
        {
            var chat = await _repository.GetChatById(messageViewModel.ChatId);

            int userId = Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value);

            var message = new Message()
            {
                ReceiverId = await _repository.GetReceiverId(messageViewModel.ChatId, userId),
                SenderId = userId,
                SentTime = DateTime.Now,
                Text = messageViewModel.Text
            };

            try
            {
                chat.Messages.Add(message);
                await _db.SaveChangesAsync();
                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Error saving to database");
            }
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
