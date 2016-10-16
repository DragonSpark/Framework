using DragonSpark.Extensions;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ImmutableArrayExtensionsTests
	{
		[Fact]
		public void Concat()
		{
			var sut = new[] { 2, 3, 4 }.ToImmutableArray();
			var second = new[] { 5, 6, 7 };
			var items = sut.Concat( second );
			var superset = items.ToImmutableHashSet();
			Assert.ProperSubset( superset, sut.ToImmutableHashSet() );
			Assert.ProperSubset( superset, second.ToImmutableHashSet() );
		}
	}
}