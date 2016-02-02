using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Entity
{
	public class EntityContext : DbContext, IEntityInstallationStorage
	{
		readonly LocalStoragePropertyProcessor processor;
		public event EventHandler Saved = delegate { };

		public event EventHandler Saving = delegate { };

		protected EntityContext( [Required]LocalStoragePropertyProcessor processor )
		{
			this.processor = processor;
		}

		public IDbSet<InstallationEntry> Installations { get; set; }

		protected override DbEntityValidationResult ValidateEntity( DbEntityEntry entityEntry, IDictionary<object, object> items )
		{
			// Addressing issue with: http://stackoverflow.com/questions/6038541/ef-validation-failing-on-update-when-using-lazy-loaded-required-properties
			switch ( entityEntry.State )
			{
				case EntityState.Modified:
				case EntityState.Unchanged:
					var names = entityEntry.Entity.GetType().GetProperties().Where( x => x.Has<RequiredAttribute>() ).Select( x => x.Name ).ToArray();
					names.Any().IsTrue( () => this.Load( entityEntry.Entity, names, 0, false ) );
					break;
			}

			var result = base.ValidateEntity( entityEntry, items );
			return result;
		}

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// </summary>
		/// <returns>The number of objects written to the underlying database.</returns>
		public override int SaveChanges()
		{
			Saving( this, EventArgs.Empty );

			var result = base.SaveChanges();
			if ( result > 0 )
			{
				Saved( this, EventArgs.Empty );
			}
			return result;
		}

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			processor.Process( this, modelBuilder );

			base.OnModelCreating( modelBuilder );

			var method = modelBuilder.GetType().GetMethod( "ComplexType" );

			this.GetDeclaredEntityTypes().First().Assembly.GetTypes().Where( x => x.Has<ComplexTypeAttribute>() ).Each( x =>
			{
				var info = method.MakeGenericMethod( x );
				info.Invoke( modelBuilder, null );
			} );
		}
	}
}