using Atut.Models;

namespace Atut.Services
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly IdentityDbContext _databaseContext;

        public DatabaseManager(IdentityDbContext databaseContext)
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
    }
}
