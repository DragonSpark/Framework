namespace DragonSpark.Configuration
{
	public interface IValueStore
	{
		object Get( string key );
	}
}