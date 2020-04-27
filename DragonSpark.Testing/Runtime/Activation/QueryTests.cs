using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime.Activation
{
	public sealed class QueryTests
	{
		[Fact]
		public void Verify()
		{
			var numbers = new[] {1, 2, 3, 4, 5};

			var expected = numbers.Skip(3).ToArray();

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(3).ToArray())
			     .Get(numbers)
			     .Should()
			     .Equal(expected);
		}
	}
}