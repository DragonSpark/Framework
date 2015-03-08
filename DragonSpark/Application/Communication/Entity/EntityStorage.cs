using System.Data.Entity;
using DragonSpark.Application.Communication.Entity.Notifications;
using DragonSpark.Application.Communication.Security;
using ApplicationUser = DragonSpark.Application.Communication.Security.ApplicationUser;

namespace DragonSpark.Application.Communication.Entity
{
    public abstract class EntityStorage<TUser> : DbContext, IEntityStorage where TUser : ApplicationUser
	{
		public DbSet<Role> Roles { get; set; }

		public DbSet<TUser> Users { get; set; }

		/*public DbSet<Notification> Notifications { get; set; }*/

		public DbSet<InstallationEntry> Installations { get; set; }
	}
}