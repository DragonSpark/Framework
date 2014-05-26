using DragonSpark.Extensions;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Linq;

namespace DragonSpark.Entity
{
	public class InstallationContext
	{
		readonly CompositionContainer container;

		public InstallationContext( CompositionContainer container )
		{
			this.container = container;
		}

		public void Install<TContext>( TContext context ) where TContext : DbContext, IEntityStorage
		{
			var installers = container.GetExportedValues<IInstaller>().OrderBy( x => x.Version ).Where( x => x.ContextType == typeof(TContext) && context.Installations.Find( x.Id, x.Version.ToString() ) == null ).ToArray();
			installers.Apply( x =>
			{
				x.Steps.Apply( y => y.Execute( context ) );
				context.Installations.Add( new InstallationEntry { Id = x.Id, InstallationDate = DateTime.Now, VersionStorage = x.Version.ToString() } );
				context.SaveChanges();
			} );
		}
	}
}