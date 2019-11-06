using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class UnionTests
	{
		[Fact]
		void Verify()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Union(Sequence.From(second))
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Union(second));
		}

		[Fact]
		void VerifyBody()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(4)
			     .Union(Sequence.From(second))
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Skip(4).Union(second));
		}

		[Fact]
		void VerifyBodyFirst()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(4)
			     .Union(Sequence.From(second))
			     .Skip(3)
			     .FirstOrDefault()
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).Skip(3).First());

			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(4)
			     .Union(Sequence.From(second))
			     .FirstOrDefault()
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).First());
		}
	}
}