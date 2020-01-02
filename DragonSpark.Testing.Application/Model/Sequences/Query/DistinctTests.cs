﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class DistinctTests
	{
		readonly static int[] Numbers
			= {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

		public class Benchmarks
		{
			readonly IEnumerable<int>      _classic;
			readonly ISelect<int[], int[]> _subject;

			public Benchmarks() : this(Numbers.Distinct(), Start.A.Selection.Of.Type<int>()
			                                                    .As.Sequence.Open.By.Self.Query()
			                                                    .Distinct()
			                                                    .Out()) {}

			public Benchmarks(IEnumerable<int> classic, ISelect<int[], int[]> subject)
			{
				_classic = classic;
				_subject = subject;
			}

			[Benchmark]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _subject.Get(Numbers);
		}

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Distinct()
			     .Out()
			     .Get(Numbers)
			     .Should()
			     .Equal(Numbers.Distinct());
		}
	}
}