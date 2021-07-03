using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Memory
{
	// ReSharper disable once TestFileNameWarning
	public sealed class DistinctTests
	{
		readonly static int[] Numbers
			= {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

		public class Benchmarks
		{
			readonly IEnumerable<int>      _classic;
			readonly ISelect<int[], int[]> _subject;

			public Benchmarks() : this(Numbers.Distinct(), Start.A.Selection.Of.Type<int>()
			                                                    .As.Sequence.Open.By.Self
			                                                    .Select(x => x.Distinct().ToArray())
			                                                    .Get()) {}

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
		public void Verify()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Distinct().ToArray())
			     .Get(Numbers)
			     .Should()
			     .Equal(Numbers.Distinct());
		}
	}
}