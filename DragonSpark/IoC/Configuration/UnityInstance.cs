using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Instance" )]
	public class UnityInstance : IContainerConfigurationCommand
	{
		public string BuildName { get; set; }

		public Type RegistrationType { get; set; }

		public object Instance { get; set; }

		public LifetimeManager Lifetime { get; set; }

		protected virtual object ResolveInstance( IUnityContainer container )
		{
			return Instance;
		}

		void IContainerConfigurationCommand.Configure( IUnityContainer container )
		{
			Configure( container );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected virtual void Configure( IUnityContainer container )
		{
			var instance = ResolveInstance( container );
			var type = RegistrationType ?? instance.Transform( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			container.RegisterInstance( type, BuildName, registration, Lifetime.Transform( item => item.Instance, () => new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() ) );
		}
	}
}