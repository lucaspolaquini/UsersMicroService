using GeekBurger.Users.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users
{
    public class RestrictionsContext : DbContext
    {
        public RestrictionsContext(DbContextOptions<RestrictionsContext> options) : base(options)
        {
        }

        public DbSet<Restriction> Restrictions { get; set; }
    }
}