using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Hubs;
using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using RealTimeChat.Services.Repository;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Controllers
{
    public class DirectController : Controller
    {
        private readonly IRepository _repository;
        private readonly IHubContext<ChatHub> _hubContext;
        public DirectController(ChatAppContext shoppingContext,IHubContext<ChatHub> context)
        {
            _repository = new Repository(shoppingContext);
            _hubContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateChat(int receiverId)
        {
            var getChat = await _repository.CreateChat(GetUserId(), receiverId);

            return getChat != null ? Ok(true):Ok(false);
        }
        [HttpGet]
        public async Task<IEnumerable<Chat>> GetChats()
        {
            return await _repository.GetUserChats(GetUserId());
        }
        [HttpGet]
        public async Task<IActionResult> GetChat(int receiverId)
        {
            var chat = await _repository.FindChatByUsersId(receiverId, GetUserId());

            return Ok(chat);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageViewModel messageViewModel)
        {
            var chat = await _repository.GetChatById(messageViewModel.ChatId);
            int senderId=GetUserId();
            int receiverId=chat.UserFirstId!=GetUserId()?chat.UserFirstId:chat.UserSecondId;

            if(chat.UserFirstId==GetUserId())

            messageViewModel.ReceiverId = receiverId;
            messageViewModel.SenderId = senderId;
            try
            {
                await _hubContext.Clients.Users(new List<string>() { senderId.ToString(), receiverId.ToString() }).SendAsync("ReceiveMessage", messageViewModel);
                await _repository.SendMessage(messageViewModel);
                return Ok(messageViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private int GetUserId() => Int32.Parse(User.Claims.FirstOrDefault(i => i.Type == "Id").Value);

    }
}
