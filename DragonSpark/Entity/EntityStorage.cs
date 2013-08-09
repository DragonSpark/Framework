using DragonSpark.Security;
using System.Data.Entity;

namespace DragonSpark.Entity
{
    public abstract class EntityStorage<TUser> : DbContext, IEntityStorage where TUser : UserProfile
	{
		public DbSet<TUser> Users { get; set; }

		public DbSet<InstallationEntry> Installations { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			LocalStoragePropertyProcessor.Instance.Process( this, modelBuilder );

			base.OnModelCreating( modelBuilder );
		}
	}
}