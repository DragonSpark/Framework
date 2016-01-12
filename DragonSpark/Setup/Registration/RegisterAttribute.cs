using System;

namespace DragonSpark.Setup.Registration
{
	public static class Register
	{
		public sealed class TypeAttribute : RegistrationBaseAttribute
		{
			public TypeAttribute( string name = null ) : base( t => new TypeRegistration( t, name ) ) { }
		}

		public sealed class AsAttribute : RegistrationBaseAttribute
		{
			public AsAttribute( Type @as ) : this( @as, null ) { }

			public AsAttribute( string name ) : this( null, name ) { }

			AsAttribute( Type @as, string name ) : base( t => new TypeRegistration( @as ?? t, t, name ) ) { }
		}

		public class FactoryAttribute : RegistrationBaseAttribute
		{
			public FactoryAttribute() : base( t => new FactoryRegistration( t ) ) { }
		}
	}
}