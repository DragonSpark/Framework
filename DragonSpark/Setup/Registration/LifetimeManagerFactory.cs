using System;
using DragonSpark.Activation;
using DragonSpark.Activation.Build;
using DragonSpark.Activation.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public class LifetimeManagerFactory<T> : LifetimeManagerFactory where T : LifetimeManager
	{
		public LifetimeManagerFactory()
		{}

		public LifetimeManagerFactory( IActivator activator ) : base( activator )
		{}

		public LifetimeManagerFactory( IActivator activator, ISingletonLocator locator ) : base( activator, locator )
		{}

		protected override Type DetermineType( ActivateParameter parameter )
		{
			var determineType = base.DetermineType( parameter );
			return determineType ?? typeof(T);
		}
	}
}