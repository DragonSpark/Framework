namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface ICache<in TInstance, TValue> : IAssignableReferenceSource<TInstance, TValue>
	{
		bool Contains( TInstance instance );
		
		bool Remove( TInstance instance );
	}

	public interface ICache<T> : ICache<object, T>, IAssignableReferenceSource<T> {}
}