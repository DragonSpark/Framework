using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instance) )]
	public class UnityInstance : UnityRegistrationCommand
	{
		public object Instance { [return: Required]get; set; }

		protected override void OnExecute( object parameter )
		{
			var type = RegistrationType ?? Instance.With( item => item.GetType() );
			var registration = Instance.ConvertTo( type );

			var registry = new ServiceRegistry( Container, Lifetime );
			registry.Register( new InstanceRegistrationParameter( type, registration, BuildName ) );
		}
	}
}