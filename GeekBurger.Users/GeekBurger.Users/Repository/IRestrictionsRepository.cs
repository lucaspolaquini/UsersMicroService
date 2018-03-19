using GeekBurger.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Repository
{
    public interface IRestrictionsRepository
    {
        IEnumerable<Restriction> GetFoodRestrictionsByUser(Guid userId);
        bool Add(Restriction restriction);
        void Save();
    }
}
