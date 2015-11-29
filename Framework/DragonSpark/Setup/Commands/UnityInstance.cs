using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Commands
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
			var tempasdf = container.GetHashCode();
			var uiasdf = Services.Location.Locator.GetHashCode();
			var also = container.Resolve<IServiceLocator>() == Services.Location.Locator;
			var temp = Services.Location.Locator == AmbientValues.Get<IServiceLocator>();
			var instance = ResolveInstance( container ).BuildUp();
			var type = RegistrationType ?? instance.Transform( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			var to = registration.GetType();
			var mapping = string.Concat( type.FullName, to != type ? $" -> {to.FullName}" : string.Empty );
			container.Resolve<ILogger>().Information( $"Registering Unity Instance: {mapping}" );
			container.RegisterInstance( type, BuildName, registration, Lifetime );
		}
	}
}