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
	public sealed class InlineProjectionTests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		public class Benchmarks
		{
			readonly IEnumerable<string> _classic = Source.Select(x => x).Where(x => x.Contains("ab"));

			readonly string[] _input;
			readonly ISelect<string[], string[]> _link = Start.A.Selection.Of.Type<string>()
			                                                  .As.Sequence.Open.By.Self
			                                                  .Select(x => x.Where(y => y.Contains("ab")).ToArray())
			                                                  .Get();

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
			     .As.Sequence.Open.By.Self.Select(x => x.Select(y => y.Length))
			     .Get(Source)
			     .Open()
			     .Should()
			     .Equal(Source.Select(x => x.Length));
		}

		[Fact]
		public void VerifySkipTakeWhereSkipTake()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(100)
			                                            .Take(900)
			                                            .Where(s => s.Contains("ab"))
			                                            .Select(s => s.Length)
			                                            .Skip(5)
			                                            .Take(10)
			                                            .ToArray())
			     .Get(Source)
			     .Should()
			     .Equal(Source.Skip(100)
			                  .Take(900)
			                  .Where(x => x.Contains("ab"))
			                  .Select(x => x.Length)
			                  .Skip(5)
			                  .Take(10));
		}

		[Fact]
		public void VerifyWhere()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Where(s => s.Contains("ab"))
			                                            .Select(s => s.Length)
			                                            .ToArray())
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length));
		}

		[Fact]
		public void VerifyWhereSkip()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Where(s => s.Contains("ab"))
			                                            .Skip(10)
			                                            .Select(s => s.Length)
			                                            .ToArray())
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Skip(10).Select(x => x.Length));
		}

		[Fact]
		public void VerifyWhereSkipTake()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Select(x => x.Where(y => y.Contains("ab"))
			                                            .Select(y => y.Length)
			                                            .Skip(5)
			                                            .Take(10)
			                                            .ToArray())
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length).Skip(5).Take(10));
		}
	}
}