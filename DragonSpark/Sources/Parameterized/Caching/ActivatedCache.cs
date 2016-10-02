namespace DragonSpark.Sources.Parameterized.Caching
{
	public class ActivatedCache<TInstance, TResult> : Cache<TInstance, TResult> where TInstance : class where TResult : class, new()
	{
		public ActivatedCache() : base( instance => new TResult() ) {}
	}
}