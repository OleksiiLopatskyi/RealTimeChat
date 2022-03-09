using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Models.DatabaseContext
{
    public class ChatAppContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public ChatAppContext(DbContextOptions<ChatAppContext>options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
