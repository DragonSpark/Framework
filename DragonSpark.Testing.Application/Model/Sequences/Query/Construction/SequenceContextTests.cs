using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Model.Sequences.Query.Construction;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using Humanizer;
using System;
using System.Linq;
using Xunit;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace DragonSpark.Testing.Application.Model.Sequences.Query.Construction
{
	public sealed class SequenceContextTests
	{
		const int skip = 10, take = 7;

		readonly static int[] data =
		{
			0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8,
			8, 9, 9, 9, 9, 9, 9, 9, 9, 9
		};

		[Fact]
		void Count()
		{
			var first = 0;
			Enumerable.Range(0, 100)
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .Select(x =>
			                  {
				                  first++;
				                  return x;
			                  })
			          .FirstOrDefault();

			first.Should().Be(2);

			var second = 0;

			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .Select(x =>
			             {
				             second++;
				             return x;
			             })
			     .FirstOrDefault()
			     .Get(Data.Default.Get());

			// second.Should().Be(2); // TODO: Re-assert.
		}

		public class Benchmarks
		{
			// ReSharper disable once NotAccessedField.Local
			readonly Func<int[], string>    _current;
			readonly ISelect<int[], string> _subject;

			public Benchmarks() : this(new StartNode<int[], int>(A.Self<int[]>())
			                           .Get(new Build.Select<int, string>(x => x.ToString()))
			                           .Get(FirstOrDefault<string>.Default)
			                           ,
			                           Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Open.By.Self.Query()
			                                .Select(x => x.ToString())
			                                .FirstOrDefault()) {}

			public Benchmarks(ISelect<int[], string> subject, Func<int[], string> current)
			{
				_subject = subject;
				_current = current;
			}

			[Benchmark(Baseline = true)]
			public string Classic() => data.Select(x => x.ToString()).FirstOrDefault();

			[Benchmark]
			public string Proposed() => _subject.Get(data);
		}

		[Fact]
		void Verify()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Take(take))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip).Take(take))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifyActivation()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.StoredActivation<Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Instantiation<Store<int>>()
			     .Get(data)
			     .Instance.Should()
			     .Equal(data);
		}

		[Fact]
		void VerifyComparison()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Query()
			     .Where(x => x > 8)
			     .Select(x => x.ToString())
			     .Get()
			     .Get(data)
			     .Open()
			     .Should()
			     .Equal(data.Where(x => x > 8).Select(x => x.ToString()));
		}

		[Fact]
		void VerifyCountBasic()
		{
			{
				var count = 0;
				Enumerable.Range(0, 100)
				          .Select(x =>
				                  {
					                  count++;
					                  return x;
				                  })
				          .Select(x =>
				                  {
					                  count++;
					                  return x;
				                  })
				          .FirstOrDefault();

				count.Should().Be(2);
			}

			{
				var count = 0;

				new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x =>
				                                                                          {
					                                                                          count++;
					                                                                          return x;
				                                                                          }))
				                                          .Get(new Build.Select<int, int>(x =>
				                                                                          {
					                                                                          count++;
					                                                                          return x;
				                                                                          }))
				                                          .Get(FirstOrDefault<int>.Default)
				                                          .Get(data);
				count.Should().Be(2);
			}
		}

		[Fact]
		void VerifySelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .NotBeEmpty()
			                                          .And.Subject.Should()
			                                          .Equal(data.Select(x => x.ToString()));
		}

		[Fact]
		void VerifySelectSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>())
				.Get(new Build.Select<int, int>(x => x + 1))
				.Get(new Build.Select<int, string>(x => x.ToString()))
				.Get()
				.Get(data)
				.Should()
				.Equal(data.Select(x => x + 1).Select(x => x.ToString()));
		}

		[Fact]
		void VerifySelectSelectFirst()
		{
			var element = new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x => x + 1))
			                                                        .Get(new Build.Select<int, string
			                                                             >(x => x.ToWords()))
			                                                        .Get(FirstOrDefault<string>.Default)
			                                                        .Get(data);
			element.Should().Be(data.Select(x => x + 1).Select(x => x.ToWords()).FirstOrDefault());
		}

		[Fact]
		void VerifySkipTakeWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Take(take))
			                                          .Get(new Build.Where<int>(x => x == 5))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip).Take(take).Where(x => x == 5))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySkipWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Build.Where<int>(x => x == 5))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip).Where(x => x == 5))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySkipWhereTake()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Build.Where<int>(x => x == 5))
			                                          .Get(new Take(take))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip).Where(x => x == 5).Take(take))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifyWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 8))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .NotBeEmpty()
			                                          .And.Subject.Should()
			                                          .Equal(data.Where(x => x > 8));
		}

		[Fact]
		void VerifyWhereSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 8))
			                                          .Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get()
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x > 8).Select(x => x.ToString()));
		}
	}
}