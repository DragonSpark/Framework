using DragonSpark.Extensions;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instance) )]
	public class UnityInstance : UnityRegistrationCommand
	{
		public object Instance { get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			var instance = Instance.BuildUp();
			var type = RegistrationType ?? instance.With( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			var to = registration.GetType();
			var mapping = string.Concat( type.FullName, to != type ? $" -> {to.FullName}" : string.Empty );
			MessageLogger.Information( $"Registering Unity Instance: {mapping}" );
			Container.RegisterInstance( type, BuildName, registration, Lifetime );
		}
	}
}