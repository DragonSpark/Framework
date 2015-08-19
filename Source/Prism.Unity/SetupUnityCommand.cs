using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Modularity;
using Prism.Properties;
using System;
using System.Linq;
using Prism.Events;

namespace Prism.Unity
{
    public class SetupUnityCommand : SetupCommand
    {
	    public SetupUnityCommand()
        {
            UseDefaultConfiguration = true;
        }

        public bool UseDefaultConfiguration { get; set; }

	    protected override void Execute( SetupContext context )
        {
            context.Logger.Log(Resources.CreatingUnityContainer, Category.Debug, Priority.Low);
            var container = this.CreateContainer();
            if (container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }
            context.Register( container );

            context.Logger.Log(Resources.ConfiguringUnityContainer, Category.Debug, Priority.Low);
            this.ConfigureContainer( context );

            context.Logger.Log(Resources.ConfiguringServiceLocatorSingleton, Category.Debug, Priority.Low);
            this.ConfigureServiceLocator( context );
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        [CLSCompliant(false)]
        protected virtual IUnityContainer CreateContainer()
        {
            var result = new UnityContainer();
            return result;
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer( SetupContext context )
        {
            var container = context.Container();
            
            context.Logger.Log(Properties.Resources.AddingUnityBootstrapperExtensionToContainer, Category.Debug, Priority.Low);
            container.AddNewExtension<UnityBootstrapperExtension>();

            container.RegisterInstance(context.Logger);

            var catalog = context.Item<IModuleCatalog>();
            if ( catalog != null )
            {
                 container.RegisterInstance(catalog);
            }
            
            var instances = context.Items.Except( new object[] { context.Logger, catalog } ).Where( o => o != null ).ToArray();
            foreach ( var item in instances )
            {
                container.RegisterInstance( item.GetType(), item );
            }

	        if ( UseDefaultConfiguration )
	        {
		        context.RegisterTypeIfMissing( typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), true );
		        context.RegisterTypeIfMissing( typeof(IModuleInitializer), typeof(ModuleInitializer), true );
		        context.RegisterTypeIfMissing( typeof(IModuleManager), typeof(ModuleManager), true );
		        context.RegisterTypeIfMissing( typeof(IEventAggregator), typeof(EventAggregator), true );
	        }
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="Microsoft.Practices.ServiceLocation.ServiceLocator" />.
        /// </summary>
        protected virtual void ConfigureServiceLocator( SetupContext context )
        {
            if ( !ServiceLocator.IsLocationProviderSet )
            {
                var container = context.Container();
                var locator = container.TryResolve<IServiceLocator>();
                if ( locator != null )
                {
                    ServiceLocator.SetLocatorProvider( () => locator );   
                }
            }
        }
    }
}