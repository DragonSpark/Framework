using System.Data.Entity;
using DragonSpark.Application.Communication.Entity.Notifications;
using DragonSpark.Application.Communication.Security;

namespace DragonSpark.Application.Communication.Entity
{
    public interface IEntityStorage
    {
        DbSet<Role> Roles { get; set; }

        /*DbSet<Notification> Notifications { get; set; }*/

        DbSet<InstallationEntry> Installations { get; }
    }
}