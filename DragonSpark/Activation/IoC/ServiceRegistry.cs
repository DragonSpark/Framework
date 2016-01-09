using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using LifetimeManagerFactory = DragonSpark.Setup.Registration.LifetimeManagerFactory;

namespace DragonSpark.Activation.IoC
{
	public class ServiceRegistry : IServiceRegistry
	{
		readonly IUnityContainer container;
		readonly IFactory<ActivateParameter, LifetimeManager> lifetimeFactory;

		public ServiceRegistry( [Required]IUnityContainer container ) : this( container, container.Resolve<LifetimeManagerFactory>() ) {}

		public ServiceRegistry( [Required]IUnityContainer container, [Required]IFactory<ActivateParameter, LifetimeManager> lifetimeFactory )
		{
			this.container = container;
			this.lifetimeFactory = lifetimeFactory;
		}

		public void Register( Type @from, Type mappedTo, string name = null ) => container.Registration().Mapping( @from, mappedTo, name, lifetimeFactory.CreateUsing( mappedTo ) );

		public void Register( Type type, object instance ) => container.Registration().Instance( type, instance );

		public void RegisterFactory( Type type, Func<object> factory ) => container.RegisterType( type, new InjectionFactory( x => factory() ) );
	}
}