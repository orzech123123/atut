using Microsoft.EntityFrameworkCore;

namespace Atut.Models
{
    public class AtutDbContext : DbContext
    {
        public AtutDbContext(DbContextOptions<AtutDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
