using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class UserRetrievedMessage
    {
        public static readonly string DefaultTopic = "UserRetrieved";

        public bool AreRestrictionsSet { get; set; }

        public Guid UserId { get; set; }

        public FoodRestrictionsList Restrictions { get; set; }
    }
}
