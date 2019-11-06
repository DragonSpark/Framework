using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Model.Sequences.Query.Construction;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query.Construction
{
	public sealed class ExitTests
	{
		const int skip = 10, take = 7;

		readonly static int[] data =
		{
			0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8,
			8, 9, 9, 9, 9, 9, 9, 9, 9, 9
		};

		public class Benchmarks
		{
			readonly IEnumerable<int>      _classic;
			readonly ISelect<int[], int[]> _subject;

			public Benchmarks() : this(Start.A.Selection.Of.Type<int>()
			                                .As.Sequence.Array.By.Self.Query()
			                                .Skip(6)
			                                .Take(5)
			                                .Where(x => x > 2)
			                                .Out(),
			                           data.Skip(6).Take(5).Where(x => x > 2)) {}

			public Benchmarks(ISelect<int[], int[]> subject, IEnumerable<int> classic)
			{
				_subject = subject;
				_classic = classic;
			}

			[Benchmark(Baseline = true)]
			public Array Classic() => _classic.ToArray();

			[Benchmark]
			public Array Subject() => _subject.Get(data);
		}

		[Fact]
		void VerifyFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(11))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(5)
			                                          .And.Subject.Should()
			                                          .Be(data.Skip(11).FirstOrDefault());

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(10))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(4)
			                                          .And.Subject.Should()
			                                          .Be(data.Skip(10).FirstOrDefault());
		}

		[Fact]
		void VerifySelectSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Build.Select<string, int>(x => x.Length))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x.ToString()).Select(x => x.Length));

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Build.Select<string, int>(x => x.Length))
			                                          .Get(new Skip(5))
			                                          .Get(new Take(4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x.ToString())
			                                                     .Select(x => x.Length)
			                                                     .Skip(5)
			                                                     .Take(4))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		// ContentContainerNode

		[Fact]
		void VerifySelectSkip()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Skip(5))
			                                          .Get(new Take(4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x.ToString()).Skip(5).Take(4))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySelectSkipSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Skip(5))
			                                          .Get(new Build.Select<string, int>(x => x.Length))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x.ToString())
			                                                     .Skip(5)
			                                                     .Select(x => x.Length))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySelectSkipTake()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Skip(5))
			                                          .Get(new Take(4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x.ToString())
			                                                     .Skip(5)
			                                                     .Take(4))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySelectSkipTakeFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Skip(5))
			                                          .Get(new Take(4))
			                                          .Get(FirstOrDefault<string>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(data.Select(x => x.ToString())
			                                                  .Skip(5)
			                                                  .Take(4)
			                                                  .First());
		}

		[Fact]
		void VerifySelectWhereFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(new Build.Where<string>(x => x == "5"))
			                                          .Get(FirstOrDefault<string>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(data.Select(x => x.ToString()).First(x => x == "5"));
		}

		// ContentContainerWithBodyNode

		[Fact]
		void VerifySelectWhereWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x => x))
			                                          .Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Select(x => x)
			                                                     .Where(x => x > 3 && x < 7)
			                                                     .Where(x => x == 4));
		}

		[Fact]
		void VerifySelectWhereWhereFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Select<int, int>(x => x))
			                                          .Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(data.Select(x => x)
			                                                  .Where(x => x > 3 && x < 7)
			                                                  .First(x => x == 4));
		}

		[Fact]
		void VerifySkipSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip)
			                                                     .Select(x => x.ToString()))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		// StartWithBodyNode

		[Fact]
		void VerifySkipTake()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(skip))
			                                          .Get(new Take(take))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Skip(skip).Take(take))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifySkipTakeFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(4))
			                                          .Get(new Take(3))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(3)
			                                          .And.Subject.Should()
			                                          .Be(data.Skip(4)
			                                                  .Take(3)
			                                                  .FirstOrDefault());

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(3))
			                                          .Get(new Take(3))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(2)
			                                          .And.Subject.Should()
			                                          .Be(data.Skip(3)
			                                                  .Take(3)
			                                                  .FirstOrDefault());
		}

		[Fact]
		void VerifySkipTakeWhere()
		{
			var node = new StartNode<int[], int>(A.Self<int[]>()).Get(new Skip(6))
			                                                     .Get(new Take(5))
			                                                     .Get(new Build.Where<int>(x => x > 2));
			node.Get(data).Should().Equal(data.Skip(6).Take(5).Where(x => x > 2));

			node.Get(FirstOrDefault<int>.Default)
			    .Get(data)
			    .Should()
			    .Be(3);

			node.Get(new Skip(1))
			    .Get(FirstOrDefault<int>.Default)
			    .Get(data)
			    .Should()
			    .Be(4);
		}

		// Start

		[Fact]
		void VerifyTake()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Take(take))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Take(take))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		[Fact]
		void VerifyWhereSelect()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 8))
			                                          .Get(new Build.Select<int, string>(x => x.ToString()))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x > 8).Select(x => x.ToString()))
			                                          .And.Subject.Should()
			                                          .NotBeEmpty();
		}

		// StoreWithBodyNode

		[Fact]
		void VerifyWhereSkip()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x == 5))
			                                          .Get(new Skip(2))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x == 5).Skip(2))
			                                          .And.Subject.Should()
			                                          .HaveCount(3)
			                                          .And.Subject.Should()
			                                          .AllBeEquivalentTo(5);
		}

		[Fact]
		void VerifyWhereSkipFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x == 5))
			                                          .Get(new Skip(2))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(5);

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 6))
			                                          .Get(new Skip(4))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(5);

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 6))
			                                          .Get(new Skip(3))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(4);
		}

		[Fact]
		void VerifyWhereSkipWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Skip(3))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x > 3 && x < 7)
			                                                     .Skip(3)
			                                                     .Where(x => x == 4));

			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Skip(3))
			                                          .Get(new Take(5))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x > 3 && x < 7)
			                                                     .Skip(3)
			                                                     .Take(5)
			                                                     .Where(x => x == 4))
			                                          .And.Subject.Should()
			                                          .HaveCount(1)
			                                          .And.Subject.Should()
			                                          .AllBeEquivalentTo(4);
		}

		[Fact]
		void VerifyWhereWhere()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(data)
			                                          .Should()
			                                          .Equal(data.Where(x => x > 3 && x < 7)
			                                                     .Where(x => x == 4));
		}

		[Fact]
		void VerifyWhereWhereFirst()
		{
			new StartNode<int[], int>(A.Self<int[]>()).Get(new Build.Where<int>(x => x > 3 && x < 7))
			                                          .Get(new Build.Where<int>(x => x == 4))
			                                          .Get(FirstOrDefault<int>.Default)
			                                          .Get(data)
			                                          .Should()
			                                          .Be(data.Where(x => x > 3 && x < 7).First(x => x == 4));
		}
	}
}