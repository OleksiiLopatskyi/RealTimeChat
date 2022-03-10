using RealTimeChat.Models;
using RealTimeChat.Models.DatabaseContext;
using RealTimeChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Services.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Chat>> GetUserChats(int userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<Chat> FindChatByUsersId(int receiverId,int userId);
        Task<Chat> GetChatById(int chatId);
        Task<int> GetReceiverId(int chatId,int userId);
        Task<Chat> CreateChat(int userId, int receiverId);
        Task<Message> SendMessage(MessageViewModel model);
    }
}
