using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using Serilog;
using System.Data.Entity;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( nameof(Installers) )]
	public abstract class InitializeDatabaseCommand<TContext> : CommandBase<object> where TContext : DbContext, IEntityInstallationStorage, new()
	{
		[Service, Required]
		public IDatabaseInitializer<TContext> Initializer { [return: Required]get; set; }

		public System.Collections.ObjectModel.Collection<IInstaller> Installers { get; } = new System.Collections.ObjectModel.Collection<IInstaller>();

		[Service, Required]
		public ILogger MessageLogger { [return: Required]get; set; }

		public override void Execute( object parameter )
		{
			Database.SetInitializer( Initializer );

			using ( var context = new TContext() )
			{
				MessageLogger.Information( "Initializing Database." );
				
				context.Database.Initialize( true );

				var items = Installers.OrderBy( x => x.Version ).Where( x => x.ContextType == typeof(TContext) && context.Installations.Find( x.Id, x.Version.ToString() ) == null ).ToArray();

				MessageLogger.Information( "Performing entity installation on {Length} installers.", items.Length );

				foreach ( var x in items )
				{
					MessageLogger.Information( "Installing Entity Installer with ID of '{Id}' and version '{Version}'.", x.Id, x.Version );

					foreach ( var y in x.Steps )
					{
						y.Execute( context );
						context.Save();
					}
					
					context.Create<InstallationEntry>( y => x.MapInto( y ) );
					context.Save();
				}
				
				MessageLogger.Information( "Database Initialized." );
			}
		}
	}
}
