using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application
{
    public class ApplicationModule<TConfiguration> : ApplicationModule where TConfiguration : IContainerConfigurationCommand
	{
		public ApplicationModule( IUnityContainer container, IModuleMonitor monitor ) : base( container, monitor )
		{}

		protected override IEnumerable<IContainerConfigurationCommand> DetermineConfigurations()
		{
			var result = Container.Resolve<TConfiguration>().ToEnumerable().Cast<IContainerConfigurationCommand>().ToArray();
			return result;
		}
	}

    public class ApplicationModule : IModule
	{
		readonly IUnityContainer container;
		readonly IModuleMonitor moduleMonitor;

		public ApplicationModule( IUnityContainer container, IModuleMonitor moduleMonitor )
		{
			this.container = container;
			this.moduleMonitor = moduleMonitor;
		}

		protected IUnityContainer Container
		{
			get { return container; }
		}

		void Microsoft.Practices.Prism.Modularity.IModule.Initialize()
		{
			Initialize();

			moduleMonitor.Mark( this );
		}

		void IModule.Load()
		{
			Load();
		}

		protected virtual void Initialize()
		{}

		/*protected virtual void OnInitializing( EventArgs eventArgs )
		{}*/

		protected virtual void Load()
		{
			var configurations = DetermineConfigurations();
			configurations.Apply( x => x.Configure( container ) );
		}
		
		protected virtual IEnumerable<IContainerConfigurationCommand> DetermineConfigurations()
		{
			var result = GetType().Assembly.GetExportedTypes().OfType<IContainerConfigurationCommand>().ToArray();
			return result;
		}
	}
}