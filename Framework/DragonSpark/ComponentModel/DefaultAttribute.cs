using System;
using DragonSpark.Aspects;

namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		public DefaultAttribute( object value ) : base( () => new DefaultValueProvider( value ) )
		{}
	}
}