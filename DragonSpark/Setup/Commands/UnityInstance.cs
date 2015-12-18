using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instance) )]
	public class UnityInstance : UnityRegistrationCommand
	{
		public object Instance { get; set; }

		protected override void Configure( IUnityContainer container )
		{
			var instance = Instance.BuildUp();
			var type = RegistrationType ?? instance.With( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			var to = registration.GetType();
			var mapping = string.Concat( type.FullName, to != type ? $" -> {to.FullName}" : string.Empty );
			container.Resolve<IMessageLogger>().Information( $"Registering Unity Instance: {mapping}" );
			container.RegisterInstance( type, BuildName, registration, Lifetime );
		}
	}
}