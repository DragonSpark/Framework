using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( "Installers" )]
	public abstract class InitializeDatabaseCommand<TContext> : SetupCommand where TContext : DbContext, IEntityInstallationStorage
	{
		[Activate]
		public IDatabaseInitializer<TContext> Initializer { get; set; }

		public Collection<IInstaller> Installers { get; } = new Collection<IInstaller>();

		protected override void Execute( SetupContext context )
		{
			Initializer.With( initializer =>
			{
				Database.SetInitializer( initializer );

				using ( var instance = Activator.Create<TContext>() )
				{
					instance.Database.Initialize( true );

					var items = Installers.OrderBy( x => x.Version ).Where( x => x.ContextType == typeof(TContext) && instance.Installations.Find( x.Id, x.Version.ToString() ) == null ).ToArray();
					items.Apply( x =>
					{
						x.Steps.Apply( y =>
						{
							y.Execute( instance );
							instance.Save();
						} );
						instance.Create<InstallationEntry>( y => x.MapInto( y ) );
						instance.Save();
					} );
				}
			} );
		}
	}
}
