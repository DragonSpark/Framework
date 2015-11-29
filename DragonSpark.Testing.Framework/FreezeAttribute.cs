using DragonSpark.Activation;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	public class FreezeAttribute : RegistrationAttribute
	{
		public FreezeAttribute( Type type ) : base( type )
		{}

		public FreezeAttribute( Type @from, Type to ) : base( @from, to )
		{}

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			var customization = new FreezingCustomization( MappedTo, RegistrationType );
			fixture.Customize( customization );
		}
	}
}