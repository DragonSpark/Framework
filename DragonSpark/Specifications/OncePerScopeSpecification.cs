using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Specifications
{
	public class OncePerScopeSpecification<T> : ConditionMonitorSpecificationBase<T>
	{
		public static OncePerScopeSpecification<T> Default { get; } = new OncePerScopeSpecification<T>();
		OncePerScopeSpecification() : this( new Scope<ConditionMonitor>( Factory.GlobalCache( () => new ConditionMonitor() ) ) ) {}

		public OncePerScopeSpecification( ISource<ConditionMonitor> source ) : base( source.Wrap<T, ConditionMonitor>() ) {}
	}
}