using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Services.Repository
{
    public class Repository : IRepository
    {
        private ChatAppContext _db;

        public Repository(ChatAppContext context)
        {
            _db = context;
        }

        public async Task<Chat> CreateChat(int userId, int receiverId)
        {
            Chat chat = new Chat()
            {
                UserFirstId = userId,
                UserSecondId = receiverId
            };

            var chatExists = await _db.Chats.FirstOrDefaultAsync(i => i.UserFirstId == chat.UserFirstId && i.UserSecondId == chat.UserSecondId ||
                i.UserFirstId == chat.UserSecondId && i.UserSecondId == chat.UserFirstId);

            if (chatExists != null)
                return chatExists;
            
            await _db.Chats.AddAsync(chat);
            await _db.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> GetChatById(int chatId) => await _db.Chats.Include(i=>i.Messages).FirstOrDefaultAsync(i=>i.Id==chatId);

        public async Task<int> GetReceiverId(int chatId,int userId)
        {
            var chat = await GetChatById(chatId);
            if (chat.UserFirstId != userId)
                return chat.UserFirstId;
            else
                return chat.UserSecondId;
        }

        public async Task<User> GetUserById(int userId) => await _db.Users.Include(i=>i.Chats).ThenInclude(i=>i.Messages).FirstOrDefaultAsync(i => i.Id == userId);
       

        public async Task<IEnumerable<Chat>> GetUserChats(int userId)
        {
            var user = await GetUserById(userId);
            return _db.Chats.Where(i=>i.UserFirstId==user.Id||i.UserSecondId==user.Id);
        }
    }
}
