using System;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;

namespace DragonSpark.Setup
{
	// [ContentProperty( "Instance" )]
	public class UnityInstance : SetupCommand
	{
		public string BuildName { get; set; }

		public Type RegistrationType { get; set; }

		public object Instance { get; set; }

		public Microsoft.Practices.Unity.LifetimeManager Lifetime { get; set; }

		protected virtual object ResolveInstance( IUnityContainer container )
		{
			return Instance;
		}

		protected virtual void Configure( IUnityContainer container )
		{
			var instance = ResolveInstance( container );
			var type = RegistrationType ?? instance.Transform( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			container.RegisterInstance( type, BuildName, registration, Lifetime ?? new ContainerControlledLifetimeManager() );
		}

		protected override void Execute( SetupContext context )
		{
			Configure( context.Container() );
		}
	}
}