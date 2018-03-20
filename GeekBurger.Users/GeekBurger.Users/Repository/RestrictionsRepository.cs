using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Model;
using GeekBurger.Users.Services;

namespace GeekBurger.Users.Repository
{
    public class RestrictionsRepository : IRestrictionsRepository
    {
        private RestrictionsContext _context;

        public RestrictionsRepository(RestrictionsContext context)
        {
            _context = context;
        }

        public bool Add(Restriction restriction)
        {
            restriction.RestrictionId = Guid.NewGuid();
            _context.Restrictions.Add(restriction);
            return true;
        }

        public IEnumerable<Restriction> GetFoodRestrictionsByUser(Guid userId)
        {
            var restrictions = _context.Restrictions?.Where(r => r.UserId.Equals(userId));
            return restrictions;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
