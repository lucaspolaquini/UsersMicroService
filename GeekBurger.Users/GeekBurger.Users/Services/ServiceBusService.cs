using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    public class ServiceBusService : IServiceBus
    {
        public Task PostMessage(string topic, string message)
        {
            throw new NotImplementedException();
        }
    }
}
