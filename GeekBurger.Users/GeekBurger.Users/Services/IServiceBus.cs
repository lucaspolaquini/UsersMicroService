using GeekBurger.Users.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    interface IServiceBus
    {
        Task PostMessage(string topic, UserRetrievedMessage message);
    }
}
