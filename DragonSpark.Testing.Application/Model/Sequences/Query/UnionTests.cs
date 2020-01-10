using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
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
			     .As.Sequence.Open.By.Self.Query()
			     .Query(x => x.Union(second).ToArray())
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
			     .As.Sequence.Open.By.Self.Query()
			     .Query(x => x.Skip(4)
			                  .Union(second)
			                  .ToArray())
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
			     .As.Sequence.Open.By.Self.Query()
			     .Reduce(x => x.Skip(4)
			                   .Union(second)
			                   .Skip(3)
			                   .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).Skip(3).First());

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Reduce(x => x.Skip(4)
			                   .Union(second)
			                   .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).First());
		}
	}
}