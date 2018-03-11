using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atut.Models
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Vehicle> Vehicles { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vehicle>()
                .HasIndex(v => new { v.RegistrationNumber, v.UserId })
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
