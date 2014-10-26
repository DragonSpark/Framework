using System;

namespace DragonSpark.Testing.Framework
{
	public class FreezeAttribute : RegistrationAttribute
	{
		public FreezeAttribute( Type type ) : this( type, type )
		{}

		public FreezeAttribute( Type @from, Type to ) : base( @from, to )
		{}

		public bool After { get; set; }
	}
}