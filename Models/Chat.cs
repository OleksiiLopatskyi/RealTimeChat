using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int UserFirstId { get; set; }
        public int UserSecondId { get; set; }
        public List<Message> Messages { get; set; }
        public Chat()
        {
            Messages = new List<Message>();
        }

    }
}
