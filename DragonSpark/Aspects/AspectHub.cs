using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Aspects
{
	public sealed class AspectHub : Cache<IAspectHub>
	{
		public static AspectHub Default { get; } = new AspectHub();
		AspectHub() {}
	}
}