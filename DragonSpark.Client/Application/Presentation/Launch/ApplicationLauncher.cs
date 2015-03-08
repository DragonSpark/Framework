using DragonSpark.Application.Presentation.Configuration;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Application.Presentation.Navigation;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;

namespace DragonSpark.Application.Presentation.Launch
{
	public class ApplicationLauncher : Launcher
	{
		[ActivationDefault( typeof(InstanceFactory<UIElement>) )]
		public IFactory ShellLocator { get; set; }

		[ActivationDefault( typeof(InstanceFactory<IModuleCatalog>) )]
		public IFactory ModuleCatalogLocator { get; set; }

		IDictionary<string,string> Parameters { get; set; }

		public void Launch( IDictionary<string,string> parameters )
		{
			Parameters = parameters;
			Run();
			var e = Container.Resolve<IEventAggregator>().GetEvent<ApplicationLaunchEvent>();
			e.Publish( ApplicationLaunchStatus.Initialized );
		}

		protected override IModuleCatalog CreateModuleCatalog()
		{
			var result = ModuleCatalogLocator.Create<IModuleCatalog>() ?? base.CreateModuleCatalog();
			return result;
		}

		protected override void ConfigureContainer()
		{
			Container.RegisterInstance( typeof(ApplicationLauncher), this );
			Container.RegisterInstance( this );
			Container.RegisterInstance( Deployment.Current.Dispatcher );
			Container.RegisterInstance( Parameters );
			Container.RegisterInstance( IsolatedStorageSettings.ApplicationSettings );
			
			base.ConfigureContainer();

			var e = Container.Resolve<IEventAggregator>().GetEvent<ApplicationLaunchEvent>();
			e.Publish( ApplicationLaunchStatus.Initializing );
		}

		protected override void InitializeShell()
		{
			base.InitializeShell();

		    var application = System.Windows.Application.Current;
		    application.RootVisual = application.RootVisual ?? Shell.As<UIElement>();
		}

		protected override DependencyObject CreateShell()
		{
			var result = ShellLocator.Create( typeof(DependencyObject) ).To<DependencyObject>() ?? base.CreateShell();
			return result;
		}

		protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
		{
			var result = base.ConfigureDefaultRegionBehaviors();
			result.AddIfMissing( "RefreshOnPrincipalChangedBehavior", typeof(RefreshOnPrincipalChangedBehavior) );
			return result;
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			var result = base.ConfigureRegionAdapterMappings();
			var factory = Container.Resolve<IRegionBehaviorFactory>();
			result.RegisterMapping( typeof(Frame), new FrameRegionAdapter( factory ) );
			return result;
		}
	}
}