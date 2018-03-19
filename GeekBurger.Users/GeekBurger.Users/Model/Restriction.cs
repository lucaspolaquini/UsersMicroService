using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    public class Restriction
    {
        public Guid RestrictionId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public bool Other { get; set; }
    }
}
