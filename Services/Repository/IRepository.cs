using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Services.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Chat>> GetUserChats(int userId);
        Task<User> GetUserById(int userId);
        Task<Chat> GetChatById(int chatId);
        Task<int> GetReceiverId(int chatId,int userId);
        Task<Chat> CreateChat(int userId, int receiverId);
    }
}
