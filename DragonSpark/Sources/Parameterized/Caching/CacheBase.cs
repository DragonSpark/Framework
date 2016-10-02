namespace DragonSpark.Sources.Parameterized.Caching
{
	public abstract class CacheBase<TInstance, TValue> : ParameterizedSourceBase<TInstance, TValue>, ICache<TInstance, TValue>
	{
		public abstract void Set( TInstance instance, TValue value );
		public abstract bool Contains( TInstance instance );
		public abstract bool Remove( TInstance instance );
	}
}