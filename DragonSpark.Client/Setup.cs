using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using DragonSpark.Application.Client.Eventing;
using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Application.Client
{
	[System.Windows.Markup.ContentProperty( "Parameters" )]
	public class Setup : DragonSpark.Application.Setup
	{
		public SetupParameters Parameters { get; set; }

		public void Launch( string[] arguments = null )
		{
			Parameters.With( y =>
			{
				y.Arguments = arguments ?? y.Arguments;
				Run( y.RunWithDefaultConfiguration );
			} );
		}
		
		protected override IModuleCatalog CreateModuleCatalog()
		{
			var result = Parameters.Catalog ?? base.CreateModuleCatalog();
			return result;
		}

		protected override void ConfigureContainer()
		{
			Container.RegisterInstance( this );
			
			Container.RegisterInstance( Parameters );
			
			base.ConfigureContainer();
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			var application = System.Windows.Application.Current;
			Shell.As<Window>( window =>
			{
				Logger.Log( "Assigning and Displaying Primary Window/Shell.", Category.Info, Microsoft.Practices.Prism.Logging.Priority.None );
				application.MainWindow = window;
				window.Show();
				Logger.Log( "Primary Window/Shell Displayed.", Category.Info, Microsoft.Practices.Prism.Logging.Priority.None );
			} );

			// this.Event<ShellInitializedEvent>().Publish( application );
			Logger.Log( "Primary Window/Shell Initialized.", Category.Info, Microsoft.Practices.Prism.Logging.Priority.None );
		}

		protected override DependencyObject CreateShell()
		{
			var result = Parameters.Shell ?? base.CreateShell();
			return result;
		}

		protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
		{
			var result = base.ConfigureDefaultRegionBehaviors();
			// result.AddIfMissing( "RefreshOnPrincipalChangedBehavior", typeof(RefreshOnPrincipalChangedBehavior) );
			return result;
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var result = base.ConfigureRegionAdapterMappings();
			/*var factory = Container.Resolve<IRegionBehaviorFactory>();
			result.RegisterMapping( typeof(Frame), new FrameRegionAdapter( factory ) );*/
			return result;
		}
	}
}