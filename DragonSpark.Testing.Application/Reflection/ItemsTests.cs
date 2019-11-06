using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
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