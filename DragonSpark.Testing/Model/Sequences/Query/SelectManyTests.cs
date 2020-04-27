using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	// ReSharper disable once TestFileNameWarning
	public sealed class SelectManyTests
	{
		readonly static Numbers[] Data =
		{
			new Numbers(1, 2, 3, 4),
			new Numbers(5, 6, 7, 8),
			new Numbers(9, 10)
		};

		public sealed class Numbers
		{
			public Numbers(params int[] elements) => Elements = elements;

			public int[] Elements { get; }
		}

		public class Benchmarks
		{
			readonly IEnumerable<int>          _classic;
			readonly ISelect<Numbers[], int[]> _subject;

			public Benchmarks() : this(Data.Hide().SelectMany(x => x.Elements),
			                           Start.A.Selection<Numbers>()
			                                .As.Sequence.Open.By.Self
			                                .Select(x => x.SelectMany(numbers => numbers.Elements).ToArray())
			                                .Get()
			                          ) {}

			public Benchmarks(IEnumerable<int> classic, ISelect<Numbers[], int[]> subject)
			{
				_classic = classic;
				_subject = subject;
			}

			[Benchmark(Baseline = true)]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _subject.Get(Data);
		}

		[Fact]
		public void Verify()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Open.By.Self.Select(x => x.SelectMany(y => y.Elements).ToArray())
			     .Get(Data)
			     .Should()
			     .Equal(Data.SelectMany(x => x.Elements));
		}

		[Fact]
		public void VerifyBody()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(2)
			                                            .SelectMany(y => y.Elements)
			                                            .ToArray())
			     .Get(Data)
			     .Should()
			     .Equal(Data.Skip(2).SelectMany(x => x.Elements));
		}

		[Fact]
		public void VerifyBodyFirst()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(1)
			                                            .SelectMany(y => y.Elements)
			                                            .FirstOrDefault())
			     .Get(Data)
			     .Should()
			     .Be(Data.Skip(1).SelectMany(x => x.Elements).First());

			Start.A.Selection<Numbers>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(1)
			                                            .SelectMany(y => y.Elements)
			                                            .Skip(2)
			                                            .FirstOrDefault())
			     .Get(Data)
			     .Should()
			     .Be(Data.Skip(1).SelectMany(x => x.Elements).Skip(2).First());
		}
	}
}