using System.Data.Entity;

namespace DragonSpark.Entity
{
    public interface IEntityStorage
    {

        /*DbSet<Role> Roles { get; set; }*/

        /*DbSet<Notification> Notifications { get; set; }*/

        DbSet<InstallationEntry> Installations { get; }
    }
}