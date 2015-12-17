namespace DragonSpark.ComponentModel
{
	public class DefaultAttribute : DefaultValueBase
	{
		readonly object value;

		public DefaultAttribute( object value ) : base( typeof(DefaultValueProvider) )
		{
			this.value = value;
		}
	}
}