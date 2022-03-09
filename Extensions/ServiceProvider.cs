using Microsoft.Extensions.Configuration;
using RealTimeChat.Models.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeChat.Extensions
{
    public static class ServiceProvider
    {
        public static ChatAppContext GetContext(this IServiceProvider provider)
        {
            return (ChatAppContext)provider.GetService(typeof(ChatAppContext));
        }
    }
}
