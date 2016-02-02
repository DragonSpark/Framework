using System;

namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		public DefaultAttribute( object value ) : base( t => new DefaultValueProvider( value ) ) {}

		public DefaultAttribute( Func<object> factory ) : base( t => new DefaultValueProvider( factory ) ) {}
	}
}