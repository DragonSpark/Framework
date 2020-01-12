using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
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
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(1).ToArray())
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
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(3)
			                                            .Take(2)
			                                            .ToArray())
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
			     .As.Sequence.Open.By.Self.Select(x => x.Take(2).ToArray())
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
			     .As.Sequence.Open.By.Self
			     .Get(array)
			     .Should()
			     .Equal(array);
		}
	}
}