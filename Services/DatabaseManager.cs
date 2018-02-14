using Atut.Models;
using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseManager(DatabaseContext databaseContext)
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
