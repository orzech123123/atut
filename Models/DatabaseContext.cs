using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atut.Models
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vehicle>()
                .HasIndex(v => new { v.RegistrationNumber, v.UserId })
                .IsUnique();

            builder.Entity<Country>()
                .HasOne(c => c.Journey)
                .WithMany(j => j.Countries);

            base.OnModelCreating(builder);
        }
    }
}
