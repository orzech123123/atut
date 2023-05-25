using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atut.Models
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<JourneyVehicle> JourneyVehicles { get; set; }
        public DbSet<VatNumber> VatNumbers { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vehicle>()
                .HasIndex(v => new { v.RegistrationNumber, v.UserId })
                .IsUnique();

            builder.Entity<Country>()
                .HasOne(c => c.Journey)
                .WithMany(j => j.Countries);
            
            builder.Entity<JourneyVehicle>()
                .HasKey(jv => new { jv.JourneyId, jv.VehicleId });

            builder.Entity<JourneyVehicle>()
                .HasOne(jv => jv.Journey)
                .WithMany(j => j.JourneyVehicles)
                .HasForeignKey(jv => jv.JourneyId);

            builder.Entity<JourneyVehicle>()
                .HasOne(jv => jv.Vehicle)
                .WithMany(v => v.JourneyVehicles)
                .HasForeignKey(jv => jv.VehicleId);

            builder.Entity<Invoice>()
                .ToTable("Invoice")
                .HasOne(c => c.Journey)
                .WithMany(j => j.Invoices);

            builder.Entity<VatNumber>()
                .ToTable("VatNumbers", "vat2_atutdb");

            base.OnModelCreating(builder);
        }
    }
}
