﻿using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class IntersectTests
	{
		[Fact]
		void Verify()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Query(x => x.Intersect(second).ToArray())
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Intersect(second));
		}

		[Fact]
		void VerifyBody()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Query(x => x.Skip(3)
			                  .Intersect(second)
			                  .ToArray())
			     .Out()
			     .Get(first)
			     .Should()
			     .Equal(first.Skip(3).Intersect(second));
		}

		[Fact]
		void VerifyBodyFirst()
		{
			var first  = new[] {1, 2, 3, 4, 5};
			var second = new[] {4, 5, 6, 7, 8};

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Reduce(x => x.Skip(2)
			                   .Intersect(second)
			                   .Skip(1)
			                   .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(2).Intersect(second).Skip(1).First());

			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Reduce(x => x.Skip(2)
			                   .Intersect(second)
			                   .FirstOrDefault())
			     .Get(first)
			     .Should()
			     .Be(first.Skip(2).Intersect(second).First());
		}
	}
}