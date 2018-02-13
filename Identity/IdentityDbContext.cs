using Atut.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atut.Identity
{
    public class IdentityDbContext : IdentityDbContext<User>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
