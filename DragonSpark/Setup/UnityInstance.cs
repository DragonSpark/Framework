using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Logging;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	[ContentProperty( "Instance" )]
	public class UnityInstance : UnityRegistrationCommand
	{
		public object Instance { get; set; }

		protected virtual object ResolveInstance( IUnityContainer container )
		{
			return Instance;
		}

		protected override void Configure( IUnityContainer container )
		{
			var instance = ResolveInstance( container );
			var type = RegistrationType ?? instance.Transform( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			var to = registration.GetType();
			var mapping = string.Concat( type.FullName, to != type ? $" -> {to.FullName}" : string.Empty );
			container.Resolve<ILoggerFacade>().Log( $"Registering Unity Instance: {mapping}", Category.Debug, DragonSpark.Logging.Priority.None );
			container.RegisterInstance( type, BuildName, registration, Lifetime );
		}
	}
}