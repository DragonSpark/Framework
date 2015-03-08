namespace Common.Configuration
{
	public class ConfigurationDictionaryItem<TKey, TValue>
	{
		public virtual TKey ItemKey { get; set; }

		public virtual TValue Value { get; set; }
	}
}