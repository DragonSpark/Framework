using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Testing.Objects.Setup
{
	public class Counting : DecoratedSourceCache<int>
	{
		public static Counting Default { get; } = new Counting();
		Counting() {}
	}
}