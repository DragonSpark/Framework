using System.Windows;
using System.Windows.Markup;
using DragonSpark.Application.Runtime;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Client.Stationed.Launch
{
	[ContentProperty( "Parameters" )]
	public class ApplicationLauncher : Launcher
	{
		public LaunchParameters Parameters { get; set; }

		public void Launch( string[] arguments = null )
		{
			Parameters.With( y =>
			{
				y.Arguments = arguments ?? y.Arguments;
				Run( y.RunWithDefaultConfiguration );
			} );
		}
		
		public override void Run( bool runWithDefaultConfiguration )
		{
			base.Run( runWithDefaultConfiguration );

			Publish( ApplicationLaunchStatus.Initialized );
		}

		void Publish( ApplicationLaunchStatus status )
		{
			var e = Container.Resolve<IEventAggregator>().GetEvent<ApplicationLaunchEvent>();
			e.Publish( status );
		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			var result = Parameters.Catalog ?? base.CreateModuleCatalog();
			return result;
		}

		protected override void ConfigureContainer()
		{
			Container.RegisterInstance( this );
			
			Container.RegisterInstance( System.Windows.Application.Current.Dispatcher );
			Container.RegisterInstance( Parameters );
			
			base.ConfigureContainer();

			Publish( ApplicationLaunchStatus.Initializing );
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			Parameters.With( x =>
			{
				if ( x.LaunchShellAsDialog )
				{
					x.Shell.ShowDialog();
				}
				else
				{
					System.Windows.Application.Current.MainWindow = x.Shell;
					x.Shell.Show();
				}
			} );
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