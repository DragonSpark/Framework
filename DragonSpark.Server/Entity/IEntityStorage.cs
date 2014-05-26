using System.Data.Entity;

namespace DragonSpark.Entity
{
    public interface IEntityStorage
    {
        DbSet<InstallationEntry> Installations { get; }
    }
}