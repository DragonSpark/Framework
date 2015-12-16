using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC
{
	public class ServiceRegistry : IServiceRegistry
	{
		readonly IUnityContainer container;
		readonly IMessageLogger messageLogger;
		readonly IFactory<ActivateParameter, LifetimeManager> lifetimeFactory;

		public ServiceRegistry( IUnityContainer container, IMessageLogger messageLogger, IFactory<ActivateParameter, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.messageLogger = messageLogger;
			this.lifetimeFactory = lifetimeFactory;
		}

		public void Register( Type @from, Type mappedTo, string name = null )
		{
			var lifetimeManager = lifetimeFactory.CreateUsing( mappedTo );
			messageLogger.Information( string.Format( Resources.ServiceRegistry_Registering, @from, mappedTo, lifetimeManager?.GetType().Name ?? "Transient" ) );
			container.RegisterType( from, mappedTo, name, lifetimeManager );
		}

		public void Register( Type type, object instance )
		{
			container.RegisterInstance( type, instance );
		}

		public void RegisterFactory( Type type, Func<object> factory )
		{
			container.RegisterType( type, new InjectionFactory( x =>
			{
				var item = factory();
				return item;
			} ) );
		}
	}
}