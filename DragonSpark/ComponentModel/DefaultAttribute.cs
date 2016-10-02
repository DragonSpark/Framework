using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		public DefaultAttribute( object value ) : base( new DefaultValueProvider( value ).Wrap()  ) {}

		protected DefaultAttribute( Func<object> factory ) : base( new DefaultValueProvider( factory ).Wrap() ) {}
	}
}