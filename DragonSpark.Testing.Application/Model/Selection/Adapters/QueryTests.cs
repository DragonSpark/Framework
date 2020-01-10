using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Selection.Adapters
{
	public sealed class QueryTests
	{
		[Fact]
		void Verify()
		{
			var numbers = new[] {1, 2, 3, 4, 5};

			var expected = numbers.Skip(3).ToArray();

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Query(x => x.Skip(3).ToArray())
			     .Get(numbers)
			     .Open()
			     .Should()
			     .Equal(expected);
		}
	}
}