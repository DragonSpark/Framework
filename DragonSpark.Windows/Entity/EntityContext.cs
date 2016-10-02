using DragonSpark.Commands;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace DragonSpark.Windows.Entity
{
	public class EntityContext : DbContext, IEntityInstallationStorage
	{
		public event EventHandler Saved = delegate { };
		public event EventHandler Saving = delegate { };

		readonly ICommand<DbContextBuildingParameter> command;

		protected EntityContext() : this( DefaultCommands.Default ) {}

		protected EntityContext( ICommand<DbContextBuildingParameter> command )
		{
			this.command = command;
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
			command.Execute( new DbContextBuildingParameter( this, modelBuilder ) );

			base.OnModelCreating( modelBuilder );
		}
	}
}