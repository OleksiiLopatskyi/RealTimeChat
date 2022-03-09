using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Models
{
    public class User
    {
     
        public int Id { get; set; }
        public UserAuth AuthInfo { get; set; }
        public string Username { get; set; }
        public List<Chat> Chats { get; set; }
        public User()
        {
            Chats = new List<Chat>();
        }
    }
}
