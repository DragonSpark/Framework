using System.Windows;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

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
			
			// Container.RegisterInstance( System.Windows.Application.Current.Dispatcher );
			Container.RegisterInstance( Parameters );
			
			base.ConfigureContainer();
			
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

			var application = System.Windows.Application.Current;
			Shell.As<Window>( window =>
			{
				application.MainWindow = window;
				window.Show();
			} );

			MessagingCenter.Send( application, "DragonSpark.Application.Setup.Shell.Initialized");

			/*Parameters.With( x =>
			{
				System.Windows.Application.Current.MainWindow = x.Shell;
				x.Shell.Show();
				Debugger.Break();
				/*if ( x.LaunchShellAsDialog )
				{
					x.Shell.ShowDialog();
				}
				else
				{
				}#1#
			} );*/
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