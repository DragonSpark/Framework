using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
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
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(3)
			     .Get()
			     .Get(numbers)
			     .Open()
			     .Should()
			     .Equal(expected);
		}
	}
}