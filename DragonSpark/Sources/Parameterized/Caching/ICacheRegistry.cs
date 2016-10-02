namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface ICacheRegistry<T>
	{
		void Register( object key, ICache<T> instance );
		void Clear( object key, object instance );
	}
}