using DragonSpark.Extensions;
using DragonSpark.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace DragonSpark.Entity
{
    public abstract class EntityStorage<TUser> : DbContext, IEntityStorage where TUser : UserProfile
	{
	    public DbSet<TUser> Users { get; set; }

		public DbSet<InstallationEntry> Installations { get; set; }

	    protected override DbEntityValidationResult ValidateEntity( DbEntityEntry entityEntry, IDictionary<object, object> items )
	    {
		    var validationResult = base.ValidateEntity( entityEntry, items );
			validationResult.ValidationErrors.Any().IsFalse( () => entityEntry.Entity.As<TUser>( x =>
			{
				x.LastActivity = DateTimeOffset.UtcNow;
			} ) );
		    return validationResult;
	    }

	    protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			LocalStoragePropertyProcessor.Instance.Process( this, modelBuilder );

			modelBuilder.Entity<UserProfile>().HasMany( x => x.Claims ).WithRequired().HasForeignKey( x => x.Name );

			base.OnModelCreating( modelBuilder );
		}
	}
}