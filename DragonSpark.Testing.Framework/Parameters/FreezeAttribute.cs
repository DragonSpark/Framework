using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework.Parameters
{
	public class FreezeAttribute : RegistrationBaseAttribute
	{
		public FreezeAttribute( Type registrationType ) : this( registrationType, registrationType ) {}

		public FreezeAttribute( Type registrationType, Type mappedTo ) : base( t => new FreezeRegistration( registrationType, mappedTo ) ) {}

		public class FreezeRegistration : ICustomization
		{
			readonly Type mappedTo;
			readonly private Type registrationType;

			public FreezeRegistration( Type registrationType, Type mappedTo )
			{
				this.registrationType = registrationType;
				this.mappedTo = mappedTo;
			}

			public void Customize( IFixture fixture ) => fixture.Customize( new FreezingCustomization( mappedTo, registrationType ) );
		}
	}
}