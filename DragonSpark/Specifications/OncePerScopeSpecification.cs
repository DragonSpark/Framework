using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;

namespace DragonSpark.Specifications
{
	public class OncePerScopeSpecification<T> : ConditionMonitorSpecificationBase<T>
	{
		public OncePerScopeSpecification() : this( new SingletonScope<ConditionMonitor>( () => new ConditionMonitor() ) ) {}

		[UsedImplicitly]
		public OncePerScopeSpecification( ISource<ConditionMonitor> source ) : base( source.Wrap<T, ConditionMonitor>() ) {}
	}
}