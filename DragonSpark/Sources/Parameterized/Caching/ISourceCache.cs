namespace DragonSpark.Sources.Parameterized.Caching
{
	public interface ISourceCache<in TInstance, TValue> : ICache<TInstance, IAssignableSource<TValue>> {}
}