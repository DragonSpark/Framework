using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeManagerFactory : LifetimeManagerFactory<TransientLifetimeManager>
	{
		[InjectionConstructor]
		public LifetimeManagerFactory() : this( Activation.Activator.GetCurrent, SingletonLocator.Instance ) {}

		public LifetimeManagerFactory( Func<IActivator> activator, ISingletonLocator locator ) : base( activator, locator ) {}
	}

	public class LifetimeManagerFactory<T> : ActivateFactory<LifetimeManager> where T : LifetimeManager
	{
		readonly ISingletonLocator locator;

		[InjectionConstructor]
		public LifetimeManagerFactory() : this( Activation.Activator.GetCurrent, SingletonLocator.Instance ) {}

		public LifetimeManagerFactory( Func<IActivator> activator, ISingletonLocator locator ) : base( activator, new LifetimeFactoryParameterCoercer( activator, typeof(T) ) )
		{
			this.locator = locator;
		}

		protected override LifetimeManager Activate( ActivateParameter parameter )
		{
			var result = base.Activate( parameter ).With( manager =>
			{
				locator.Locate( parameter.Type ).With( manager.SetValue );
			} );;
			return result;
		}
	}
}