using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences
{
	public sealed class SequenceTests
	{
		[Fact]
		void Skip()
		{
			var array    = new[] {1, 2, 3};
			var expected = array.Skip(1).ToArray();

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(1)
			     .Out()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void SkipTake()
		{
			var array    = new[] {1, 2, 3, 4, 5};
			var expected = array.Skip(3).Take(2).ToArray();

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(3)
			     .Take(2)
			     .Out()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void Take()
		{
			var array    = new[] {1, 2, 3};
			var expected = array.Take(2).ToArray();

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Take(2)
			     .Out()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void Verify()
		{
			var array = new[] {1, 2, 3};

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Out()
			     .Get(array)
			     .Should()
			     .Equal(array);
		}
	}
}