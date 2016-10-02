using System.Collections.Immutable;
using Xunit;

namespace DragonSpark.Testing
{
	public class ImmutableArrayTests
	{
		[Fact]
		public void Basic()
		{
			var first = new object();
			var second = new object();

			var one = new[] { first, second }.ToImmutableHashSet();
			var two = new[] { second, first }.ToImmutableHashSet();
			
			Assert.Equal( one, two );
		}
	}
}
