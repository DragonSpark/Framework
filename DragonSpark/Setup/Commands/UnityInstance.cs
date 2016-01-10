using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(Instance) )]
	public class UnityInstance : UnityRegistrationCommand
	{
		public object Instance { [return: Required]get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			var instance = Instance.BuildUp();
			var type = RegistrationType ?? instance.With( item => item.GetType() );
			var registration = instance.ConvertTo( type );
			Container.Registration().Instance( type, registration, BuildName, Lifetime );
		}
	}
}