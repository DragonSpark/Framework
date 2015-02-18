namespace DragonSpark.Server.Legacy.Entity
{
    public interface IEntityStorage
    {

        /*DbSet<Role> Roles { get; set; }*/

        /*DbSet<Notification> Notifications { get; set; }*/

        DbSet<InstallationEntry> Installations { get; }
    }
}