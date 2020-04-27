using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	// ReSharper disable once TestFileNameWarning
	public sealed class WhereDefinitionTests
	{
		const uint Total = 1000;
		const int  skip  = 100, take = 100;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		public class Benchmarks
		{
			const    string              Value    = "ab";
			readonly IEnumerable<string> _classic = Source.Skip(skip).Take(take).Where(x => x.Contains(Value));

			readonly string[] _input;
			readonly ISelect<string[], string[]> _link = Start.A.Selection.Of.Type<string>()
			                                                  .As.Sequence.Open.By.Self.Select(x => x.Skip(skip)
			                                                                                         .Take(take)
			                                                                                         .Where(y => y
				                                                                                                .Contains(Value))
			                                                                                         .ToArray())
			                                                  .Out();

			public Benchmarks() : this(Source.ToArray()) {}

			public Benchmarks(string[] input) => _input = input;

			[Benchmark(Baseline = true)]
			public string[] Classic() => _classic.ToArray();

			[Benchmark]
			public string[] Subject() => _link.Get(_input);
		}

		[Fact]
		public void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Select(y => y.Length).ToArray())
			     .Get(Source)
			     .Should()
			     .Equal(Source.Select(x => x.Length));
		}

		[Fact]
		public void VerifyDoubleWhere()
		{
			var numbers = new[] {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Where(y => y > 5)
			                                            .Where(i => i < 7)
			                                            .ToArray())
			     .Out()
			     .Get(numbers)
			     .Should()
			     .Equal(numbers.Where(x => x > 5).Where(x => x < 7));
		}

		[Fact]
		public void VerifyDoubleWhereSkipTake()
		{
			var numbers = new[] {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(10)
			                                            .Take(12)
			                                            .Where(y => y > 5)
			                                            .Where(y => y < 7)
			                                            .Skip(3)
			                                            .Take(3)
			                                            .ToArray())
			     .Out()
			     .Get(numbers)
			     .Should()
			     .Equal(numbers.Skip(10).Take(12).Where(x => x > 5).Where(x => x < 7).Skip(3).Take(3));
		}

		[Fact]
		public void VerifySkipTakeWhere()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(skip)
			                                            .Take(take)
			                                            .Where(s => s.Contains("ab"))
			                                            .ToArray())
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Skip(skip).Take(take).Where(x => x.Contains("ab")).ToArray());
		}
	}
}