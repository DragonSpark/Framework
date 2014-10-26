using System;

namespace DragonSpark.Testing.Framework
{
	public class RegisterAttribute : RegistrationAttribute
	{
		public RegisterAttribute( Type @from, Type to ) : base( @from, to )
		{}
	}
}