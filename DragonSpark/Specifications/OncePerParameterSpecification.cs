using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Specifications
{
	public class OncePerParameterSpecification<T> : ConditionMonitorSpecificationBase<T> where T : class
	{
		public OncePerParameterSpecification() : this( new Condition<T>() ) {}

		public OncePerParameterSpecification( ICache<T, ConditionMonitor> cache ) : base( cache.ToDelegate() ) {}
	}
}