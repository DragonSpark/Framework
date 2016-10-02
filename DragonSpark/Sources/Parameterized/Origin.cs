using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class Origin : Cache<ISource>
	{
		public static IAssignableReferenceSource<ISource> Default { get; } = new Origin();
		Origin() {}
	}
}