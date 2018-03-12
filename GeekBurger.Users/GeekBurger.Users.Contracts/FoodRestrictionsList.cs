using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    /// <summary>
    /// Class used to describe the structure of a restriction list
    /// </summary>
    public class FoodRestrictionsList
    {
        /// <summary>
        /// Restrictions customized by the user
        /// </summary>
        public string[] Others { get; set; }

        /// <summary>
        /// Known restrictions by the system
        /// </summary>
        public string[] Restrictions { get; set; }
    }
}
