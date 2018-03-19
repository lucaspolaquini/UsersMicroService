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
        private IRestrictionChangedService _restrictionChangedService;

        public RestrictionsRepository(RestrictionsContext context, IRestrictionChangedService restrictionChangedService)
        {
            _context = context;
            _restrictionChangedService = restrictionChangedService;
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
            _restrictionChangedService.AddToMessageList(_context.ChangeTracker.Entries<Restriction>());
            _context.SaveChanges();
            _restrictionChangedService.SendMessagesAsync();
        }
    }
}
