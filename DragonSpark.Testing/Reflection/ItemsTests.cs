using DragonSpark.Model;
using FluentAssertions;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Reflection
{
	public class ItemsTests
	{
		[Fact]
		public void Coverage()
		{
			Empty<int>.Enumerable.Should()
			          .BeSameAs(Enumerable.Empty<int>());
			Empty<int>.Immutable.Should().BeEquivalentTo(ImmutableArray<int>.Empty);
		}
	}
}