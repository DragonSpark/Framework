using DragonSpark.Activation;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	public class RegisterAttribute : RegistrationAttribute
	{
		/*public RegisterAttribute( Type registrationType ) : base( registrationType )
		{}*/

		public RegisterAttribute( Type registrationType, Type mappedTo ) : base( registrationType, mappedTo )
		{}

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			registry.Register( RegistrationType, MappedTo );
		}
	}
}