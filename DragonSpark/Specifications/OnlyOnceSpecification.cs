using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Specifications
{
	public sealed class OnlyOnceSpecification : OnlyOnceSpecification<object> {}

	public class OnlyOnceSpecification<T> : ConditionMonitorSpecificationBase<T>
	{
		public OnlyOnceSpecification() : this( new ConditionMonitor() ) {}

		public OnlyOnceSpecification( ConditionMonitor monitor ) : base( monitor.Wrap<T, ConditionMonitor>() ) {} 
	}
}