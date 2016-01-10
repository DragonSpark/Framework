using DragonSpark.Extensions;
using System;

namespace DragonSpark.Setup.Commands
{
	public class UnityType : UnityRegistrationCommand
	{
		public Type MapTo { get; set; }
		
		protected override void OnExecute( IApplicationSetupParameter parameter ) => Container.Registration().Mapping( RegistrationType, MapTo ?? RegistrationType, BuildName, Lifetime );
	}
}