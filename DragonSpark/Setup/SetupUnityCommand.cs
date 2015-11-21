using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	public class SetupUnityCommand : SetupCommand
	{
		[Default( true )]
		public bool UseDefaultConfiguration { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Logger.Information( Resources.CreatingUnityContainer, Priority.Low);
			var container = this.CreateContainer( context );
			if (container == null)
			{
				throw new InvalidOperationException(Resources.NullUnityContainerException);
			}
			context.Register( container );
			container.RegisterInstance( context );

			context.Logger.Information(Resources.ConfiguringUnityContainer, Priority.Low);
			this.ConfigureContainer( context );

			context.Logger.Information(Resources.ConfiguringServiceLocatorSingleton,Priority.Low);
			this.ConfigureServiceLocator( context );
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
			var result = new Activation.IoC.ServiceLocator( container, context.Logger );
			return result;
		}

		/// <summary>
		/// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
		/// type mappings required by the application.
		/// </summary>
		protected virtual void ConfigureContainer( SetupContext context )
		{
			var container = context.Container();
			
			context.Logger.Information(Resources.AddingUnityBootstrapperExtensionToContainer, Priority.Low);
			
			container.RegisterInstance(context.Logger);

			var catalog = context.Item<IModuleCatalog>();
			if ( catalog != null )
			{
				 container.RegisterInstance(catalog);
			}
			
			var instances = context.Items.Except( new object[] { context.Logger, catalog, container } ).Where( o => o != null ).ToArray();
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

		/// <summary>
		/// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
		/// </summary>
		protected virtual void ConfigureServiceLocator( SetupContext context )
		{
			if ( !Services.Location.IsAvailable() )
			{
				context.Container()
					.TryResolve<IServiceLocator>()
					.With( Services.Location.Assign );
			}
		}
	}
}