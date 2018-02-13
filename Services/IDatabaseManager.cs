using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public interface IDatabaseManager<TContext> where TContext : DbContext
    {
        void EnsureDatabaseCreated();
        void Commit();
        void Migrate();
    }
}