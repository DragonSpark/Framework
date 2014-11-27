using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;

namespace DragonSpark.Activation.IoC.Commands
{
	// [ContentProperty( "Instance" )]
	public class UnityInstance : IContainerConfigurationCommand
	{
		public string BuildName { get; set; }

		public Type RegistrationType { get; set; }

		public object Instance { get; set; }

		public Microsoft.Practices.Unity.LifetimeManager Lifetime { get; set; }

		protected virtual object ResolveInstance( IUnityContainer container )
		{
			return Instance;
		}

		void IContainerConfigurationCommand.Configure( IUnityContainer container )
		{
			Configure( container );
		}

		protected virtual void Configure( IUnityContainer container )
		{
			var instance = ResolveInstance( container );
			var type = RegistrationType ?? instance.Transform( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			container.RegisterInstance( type, BuildName, registration, Lifetime ?? new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() );
		}
	}
}