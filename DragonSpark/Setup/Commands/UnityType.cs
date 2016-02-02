using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using System;

namespace DragonSpark.Setup.Commands
{
	public class UnityType : UnityRegistrationCommand
	{
		public Type MapTo { get; set; }
		
		protected override void OnExecute( object parameter )
		{
			var registry = new ServiceRegistry( Container, Lifetime );
			registry.Register( new MappingRegistrationParameter( RegistrationType, MapTo ?? RegistrationType, BuildName ) );
		}
	}
}