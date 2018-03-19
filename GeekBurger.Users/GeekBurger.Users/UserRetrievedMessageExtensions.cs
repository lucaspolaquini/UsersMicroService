using GeekBurger.Users.Contract;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users
{
    public static class UserRetrievedMessageExtensions
    {
        public static string Serialize(this UserRetrievedMessage message)
        {
            return JToken.FromObject(message).ToString();
        }
    }
}
