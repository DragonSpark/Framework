using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeManagerFactory : LifetimeManagerFactory<TransientLifetimeManager>
	{
		public LifetimeManagerFactory( IActivator activator ) : this( activator, SingletonLocator.Instance )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, locator )
		{}
	}

	public class LifetimeManagerFactory<T> : ActivateFactory<LifetimeManager> where T : LifetimeManager
	{
		readonly ISingletonLocator locator;

		/*public LifetimeManagerFactory() : this( Activation.Activator.Current )
		{}*/

		public LifetimeManagerFactory( IActivator activator ) : this( activator, SingletonLocator.Instance )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, new LifetimeFactoryParameterCoercer( activator, typeof(T) ) )
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