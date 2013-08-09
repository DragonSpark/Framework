namespace DragonSpark.IoC
{
	public class LifetimeEntry
	{
		readonly object key;
		readonly object value;

		public LifetimeEntry( object item ) : this( item, item )
		{}

		public LifetimeEntry( object key, object value )
		{
			this.key = key;
			this.value = value;
		}

		public object Key
		{
			get { return key; }
		}

		public object Value
		{
			get { return value; }
		}
	}
}