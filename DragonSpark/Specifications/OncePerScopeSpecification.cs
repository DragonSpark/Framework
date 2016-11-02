using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;

namespace DragonSpark.Specifications
{
	public class OncePerScopeSpecification<T> : ConditionMonitorSpecificationBase<T>
	{
		// public static OncePerScopeSpecification<T> Default { get; } = new OncePerScopeSpecification<T>();
		public OncePerScopeSpecification() : this( new Scope<ConditionMonitor>( Factory.GlobalCache( () => new ConditionMonitor() ) ) ) {}

		[UsedImplicitly]
		public OncePerScopeSpecification( ISource<ConditionMonitor> source ) : base( source.Wrap<T, ConditionMonitor>() ) {}
	}
}