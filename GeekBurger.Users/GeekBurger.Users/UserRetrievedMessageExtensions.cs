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
        public static byte[] Serialize(this UserRetrievedMessage message)
        {
            string inJson = JToken.FromObject(message).ToString();
            return System.Text.Encoding.UTF8.GetBytes(inJson);
        }
    }
}
