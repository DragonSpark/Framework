using System;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeManagerFactory : LifetimeManagerFactory<TransientLifetimeManager>
	{
		[InjectionConstructor]
		public LifetimeManagerFactory() : this( AttributeProvider.Instance, Activation.Activator.GetCurrent, SingletonLocator.Instance ) {}

		public LifetimeManagerFactory( IAttributeProvider provider, Func<IActivator> activator, ISingletonLocator locator ) : base( provider, activator, locator ) {}
	}

	public class LifetimeManagerFactory<T> : ActivateFactory<LifetimeManager> where T : LifetimeManager
	{
		readonly ISingletonLocator locator;

		[InjectionConstructor]
		public LifetimeManagerFactory() : this( AttributeProvider.Instance, Activation.Activator.GetCurrent, SingletonLocator.Instance ) {}

		public LifetimeManagerFactory( IAttributeProvider provider, Func<IActivator> activator, ISingletonLocator locator ) : base( activator, new LifetimeFactoryParameterCoercer( provider, activator, typeof(T) ) )
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