﻿using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences
{
	public sealed class ArraySequenceTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self
			     .Out()
			     .Get(expected)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self
			     .Select(x => x.Skip(5000).Take(300).ToArray())
			     .Get(source)
			     .Should()
			     .Equal(expected);
		}
	}
}