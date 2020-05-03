using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using NetFabric.Hyperlinq;
using System.Linq;
using Xunit;
using Array = System.Array;
using Enumerable = System.Linq.Enumerable;

namespace DragonSpark.Testing.Model.Sequences
{
	// ReSharper disable once TestFileNameWarning
	public sealed class WhereTests
	{
		public class Benchmarks
		{
			const uint Total = 10_000u;
			readonly static ISelect<uint[], uint[]> Select = Start.A.Selection<uint>()
			                                                      .As.Sequence.Open.By.Self
			                                                      .Select(x => x.Where(y => y > 1000).ToArray())
			                                                      .Out();

			uint _count = Total;

			uint[] _source = default!;

			[Params(Total)]
			public uint Count
			{
				get => _count;
				set
				{
					_count  = value;
					_source = FixtureInstance.Default.Many<uint>(_count).Get().ToArray();
				}
			}

			[Benchmark]
			public Array Full() => Select.Get(_source);

			[Benchmark]
			public Array FullClassic() => _source.Where(x => x > 1000).ToArray();
		}

		[Fact]
		public void Verify()
		{
			const uint count = 1_000u;
			var array = Numbers.Default.Open()
			                   .Then()
			                   .Subject.Select(x => x.AsValueEnumerable()
			                                         .Skip((int)(count - 5))
			                                         .Take(5)
			                                         .ToArray())
			                   .Return()
			                   .Get(count);
			array.Should().HaveCount(5);

			Numbers.Default.Get(count).Open().Skip((int)(count - 5)).Take(5).ToArray().Sum().Should().Be(array.Sum());
		}

		[Fact]
		public void VerifyAdvanced()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(300)
			                                            .Take(100)
			                                            .Where(y => y > 100)
			                                            .ToArray())
			     .Out()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(300)
			                  .Take(100)
			                  .Where(x => x > 100)
			                  .ToArray());
		}

		[Fact]
		public void VerifyComprehensive()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(300)
			                                            .Take(200)
			                                            .Where(i => i > 100)
			                                            .Skip(50)
			                                            .Take(100)
			                                            .ToArray())
			     .Out()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(300)
			                  .Take(200)
			                  .Where(x => x > 100)
			                  .Skip(50)
			                  .Take(100)
			                  .ToArray());
		}

		[Fact]
		public void VerifyCount()
		{
			var source   = Enumerable.Range(0, 1_000).ToArray();
			var expected = source.Where(x => x > 100).ToArray();
			var ints = Start.A.Selection<int>()
			                .As.Sequence.Open.By.Self.Select(x => x.Where(y => y > 100).ToArray())
			                .Out()
			                .Get(source);
			ints.Should().NotBeSameAs(source);
			ints.Should().Equal(expected);
			ints.Should().HaveCountGreaterThan(500);
		}

		[Fact]
		public void VerifyWhereLink()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Where(y => y > 3).ToArray())
			     .Get(numbers)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		public void VerifyWhereSkipTake()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			var count  = 850;
			source.Where(x => x > 100)
			      .Skip(count)
			      .Take(5)
			      .ToArray()
			      .Should()
			      .Equal(Start.A.Selection<int>()
			                  .As.Sequence.Open.By.Self.Select(x => x.Where(i => i > 100)
			                                                         .Skip(count)
			                                                         .Take(5)
			                                                         .ToArray())
			                  .Out()
			                  .Get(source));
		}

		[Fact]
		public void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual = Start.A.Selection<int>()
			                  .As.Sequence.Open.By.Self.Select(x => x.Where(y => y > 3)
			                                                         .Take(1)
			                                                         .ToArray())
			                  .Out()
			                  .Get(numbers);
			actual.Should().Equal(expected);
			actual.Should().NotBeSameAs(numbers);
		}
	}
}