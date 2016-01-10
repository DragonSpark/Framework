namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		public DefaultAttribute( object value ) : base( t => new DefaultValueProvider( value ) )
		{}
	}
}