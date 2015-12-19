﻿using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using PostSharp.Patterns.Contracts;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( "Installers" )]
	public abstract class InitializeDatabaseCommand<TContext> : SetupCommand where TContext : DbContext, IEntityInstallationStorage, new()
	{
		[Activate]
		public IDatabaseInitializer<TContext> Initializer { get; set; }

		public Collection<IInstaller> Installers { get; } = new Collection<IInstaller>();

		[Activate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }

		protected override void Execute( SetupContext context )
		{
			Initializer.With( initializer =>
			{
				Database.SetInitializer( initializer );

				using ( var instance = new TContext() )
				{
					MessageLogger.Information( "Initializing Database.", Priority.Low );
					instance.Database.Initialize( true );

					var items = Installers.OrderBy( x => x.Version ).Where( x => x.ContextType == typeof(TContext) && instance.Installations.Find( x.Id, x.Version.ToString() ) == null ).ToArray();

					MessageLogger.Information( $"Performing entity installation on {items.Length} installers.", Priority.Low );

					items.Each( x =>
					{
						MessageLogger.Information( $"Installing Entity Installer with ID of '{x.Id}' and version '{x.Version}'.", Priority.Low );

						x.Steps.Each( y =>
						{
							y.Execute( instance );
							instance.Save();
						} );
						instance.Create<InstallationEntry>( y => x.MapInto( y ) );
						instance.Save();
					} );
					MessageLogger.Information( "Database Initialized.", Priority.Low );
				}
			} );
		}
	}
}
