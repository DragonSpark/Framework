using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
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
			                                .As.Sequence.Array.By.Self.Query()
			                                .SelectManyBy(x => x.Elements)
			                                .Out()) {}

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
		void Verify()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Array.By.Self.Query()
			     .SelectManyBy(x => x.Elements)
			     .Out()
			     .Get(Data)
			     .Should()
			     .Equal(Data.SelectMany(x => x.Elements));
		}

		[Fact]
		void VerifyBody()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(2)
			     .SelectManyBy(x => x.Elements)
			     .Out()
			     .Get(Data)
			     .Should()
			     .Equal(Data.Skip(2).SelectMany(x => x.Elements));
		}

		[Fact]
		void VerifyBodyFirst()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(1)
			     .SelectManyBy(x => x.Elements)
			     .FirstOrDefault()
			     .Get(Data)
			     .Should()
			     .Be(Data.Skip(1).SelectMany(x => x.Elements).First());

			Start.A.Selection<Numbers>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(1)
			     .SelectManyBy(x => x.Elements)
			     .Skip(2)
			     .FirstOrDefault()
			     .Get(Data)
			     .Should()
			     .Be(Data.Skip(1).SelectMany(x => x.Elements).Skip(2).First());
		}
	}
}