using DragonSpark.Extensions;
using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ConventionBasedFormattingExtensionsTests
	{
		[Fact]
		public void Verify()
		{
			var sut = "thisIsATest".SplitCamelCase().ToImmutableHashSet();
			Assert.Equal( new[] { "This", "Is", "A", "Test" }.ToImmutableHashSet(), sut );
		}
	}
}