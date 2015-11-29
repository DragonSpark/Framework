using System;
using System.Linq;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Commands
{
	public class SetupUnityCommand : SetupCommand
	{
		[Default( true )]
		public bool UseDefaultConfiguration { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Logger.Information( Resources.CreatingUnityContainer, Priority.Low );
			var container = this.CreateContainer( context );
			if ( container == null )
			{
				throw new InvalidOperationException( Resources.NullUnityContainerException );
			}
			context.Register( container );

			context.Logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );
			this.ConfigureContainer( context, container );
		}

		protected virtual IUnityContainer NewContainer()
		{
			var result = new UnityContainer();
			return result;
		}

		protected virtual IUnityContainer CreateContainer( SetupContext context )
		{
			var locator = CreateServiceLocator( context );
			var result = locator.GetInstance<IUnityContainer>();
			return result;
		}

		protected virtual IServiceLocator CreateServiceLocator( SetupContext context )
		{
			var container = NewContainer();
			var result = new Activation.IoC.ServiceLocator( container );
			return result;
		}

		protected virtual void ConfigureContainer( SetupContext context, IUnityContainer unityContainer )
		{
			var container = context.Container();
			container.RegisterInstance( context );
			
			context.Logger.Information( Resources.AddingUnityBootstrapperExtensionToContainer, Priority.Low );

			container.RegisterInterfaces( context.Logger );

			var setup = context.Item<ISetup>().With( x => container.RegisterInstance( x ) );
			var catalog = context.Item<IModuleCatalog>().With( x => container.RegisterInstance( x ) );
			
			var instances = context.Items.Except( new object[] { context.Logger, catalog, container, setup } ).NotNull().ToArray();
			foreach ( var item in instances )
			{
				container.RegisterInstance( item.GetType(), item );
			}

			if ( UseDefaultConfiguration )
			{
				context.RegisterTypeIfMissing( typeof(IServiceLocator), typeof(UnityServiceLocator), true );
				context.RegisterTypeIfMissing( typeof(IModuleInitializer), typeof(ModuleInitializer), true );
				context.RegisterTypeIfMissing( typeof(IModuleManager), typeof(ModuleManager), true );
				// context.RegisterTypeIfMissing( typeof(IEventAggregator), typeof(EventAggregator), true );
			}
		}
	}
}