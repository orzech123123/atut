namespace Atut.Services
{
    public interface IDatabaseManager
    {
        void EnsureDatabaseCreated();
        void Commit();
    }
}