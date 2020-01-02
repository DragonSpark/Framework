using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class InlineProjectionTests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		public class Benchmarks
		{
			readonly IEnumerable<string> _classic = Source.Select(x => x).Where(x => x.Contains("ab"));

			readonly string[] _input;
			readonly ISelect<string[], string[]> _link = Start.A.Selection.Of.Type<string>()
			                                                  .As.Sequence.Open.By.Self.Query()
			                                                  .SelectBy(x => x)
			                                                  .WhereBy(x => x.Contains("ab"))
			                                                  .Out();

			public Benchmarks() : this(Source.ToArray()) {}

			public Benchmarks(string[] input) => _input = input;

			[Benchmark(Baseline = true)]
			public string[] Classic() => _classic.ToArray();

			[Benchmark]
			public string[] Subject() => _link.Get(_input);
		}

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .SelectBy(x => x.Length)
			     .Get(Source)
			     .Open()
			     .Should()
			     .Equal(Source.Select(x => x.Length));
		}

		[Fact]
		void VerifySkipTakeWhereSkipTake()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Skip(100)
			     .Take(900)
			     .WhereBy(x => x.Contains("ab"))
			     .SelectBy(x => x.Length)
			     .Skip(5)
			     .Take(10)
			     .Out()
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
		void VerifyWhere()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .WhereBy(x => x.Contains("ab"))
			     .SelectBy(x => x.Length)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length));
		}

		[Fact]
		void VerifyWhereSkip()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .WhereBy(x => x.Contains("ab"))
			     .Skip(10)
			     .SelectBy(x => x.Length)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Skip(10).Select(x => x.Length));
		}

		[Fact]
		void VerifyWhereSkipTake()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .WhereBy(x => x.Contains("ab"))
			     .SelectBy(x => x.Length)
			     .Skip(5)
			     .Take(10)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length).Skip(5).Take(10));
		}
	}
}