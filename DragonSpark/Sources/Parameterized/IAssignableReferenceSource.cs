namespace DragonSpark.Sources.Parameterized
{
	public interface IAssignableReferenceSource<T> : IAssignableReferenceSource<object, T>, IParameterizedSource<T> {}
	public interface IAssignableReferenceSource<in TInstance, TValue> : IParameterizedSource<TInstance, TValue>
	{
		void Set( TInstance instance, TValue value );
	}
}