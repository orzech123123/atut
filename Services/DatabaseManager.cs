using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public class DatabaseManager<TContext> : IDatabaseManager<TContext> where TContext : DbContext
    {
        private readonly TContext _databaseContext;

        public DatabaseManager(TContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void EnsureDatabaseCreated()
        {
            _databaseContext.Database.EnsureCreated();
        }

        public void Commit()
        {
            if (_databaseContext.ChangeTracker.HasChanges())
            {
                _databaseContext.SaveChanges();
            }
        }

        public void Migrate()
        {
            _databaseContext.Database.Migrate();
        }
    }
}
