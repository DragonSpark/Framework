using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	public sealed class UnionTests
	{
		[Fact]
		public void Verify()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Union(second).ToArray())
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Union(second));
		}

		[Fact]
		public void VerifyBody()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(4)
			                                            .Union(second)
			                                            .ToArray())
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Skip(4).Union(second));
		}

		[Fact]
		public void VerifyBodyFirst()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(4)
			                                            .Union(second)
			                                            .Skip(3)
			                                            .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).Skip(3).First());

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(4)
			                                            .Union(second)
			                                            .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(4).Union(second).First());
		}
	}
}