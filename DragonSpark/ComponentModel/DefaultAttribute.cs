using System;
using DragonSpark.Aspects;

namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		readonly object value;

		public DefaultAttribute( object value ) : this( typeof(DefaultValueProvider), value )
		{}

		protected DefaultAttribute( [OfType( typeof(DefaultValueProvider) )]Type surrogateFor, object value ) : base( surrogateFor )
		{
			this.value = value;
		}
	}
}