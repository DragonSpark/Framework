﻿using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class ProjectionTests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		public class Benchmarks
		{
			readonly IEnumerable<int> _classic = Source.Select(x => x.Length);

			readonly string[] _input;
			readonly ISelect<string[], int[]> _link = Start.A.Selection.Of.Type<string>()
			                                               .As.Sequence.Open.By.Self.Query()
			                                               .Select(x => x.Length)
			                                               .Out();

			public Benchmarks() : this(Source.ToArray()) {}

			public Benchmarks(string[] input) => _input = input;

			[Benchmark(Baseline = true)]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _link.Get(_input);
		}

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Select(x => x.Length)
			     .Out()
			     .Get(Source)
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
			     .Where()
			     .By(x => x.Contains("ab"))
			     .Select(x => x.Length)
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
			     .Where()
			     .By(x => x.Contains("ab"))
			     .Select(x => x.Length)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length));
		}

		// TODO: verify.

		/*[Fact]
		void VerifyWhere()
		{
			Enumerable.Range(0, 10_000)
			          .AsParallel()
			          .ForAll(_ =>
			                  {
				                  Start.A.Selection.Of.Type<string>()
				                       .As.Sequence.Open.By.Self.Query()
				                       .Where(x => x.Contains("ab"))
				                       .Out()
				                       .Get(Source)
				                       .Should()
				                       .Equal(Source.Where(x => x.Contains("ab")), $"Iteration: {_}");
			                  });
		}*/



		[Fact]
		void VerifyWhereSkipTake()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Where()
			     .By(x => x.Contains("ab"))
			     .Select(x => x.Length)
			     .Skip(5)
			     .Take(10)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Where(x => x.Contains("ab")).Select(x => x.Length).Skip(5).Take(10));
		}
	}
}