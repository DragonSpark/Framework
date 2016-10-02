using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark
{
	public sealed class AssociatedPriority : DecoratedSourceCache<IPriorityAware>
	{
		public static AssociatedPriority Default { get; } = new AssociatedPriority();
		AssociatedPriority() {}
	}
}