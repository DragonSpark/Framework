using System;

namespace DragonSpark.Setup.Registration
{
	public class RegisterTypeAttribute : RegistrationBaseAttribute
	{
		public RegisterTypeAttribute() : base( () => RegisterByConventionType.Instance )
		{}
	}

	public sealed class RegisterAttribute : RegistrationBaseAttribute
	{
		public RegisterAttribute() : this( null, null )
		{}

		public RegisterAttribute( Type @as ) : this( @as, null )
		{}

		public RegisterAttribute( string name ) : this( null, name )
		{}

		RegisterAttribute( Type @as, string name ) : base( () => new RegistrationByConvention( @as, name ) )
		{}
	}
}