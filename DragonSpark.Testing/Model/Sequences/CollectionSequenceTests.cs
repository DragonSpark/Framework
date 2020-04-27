using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences
{
	// ReSharper disable once TestFileNameWarning
	public sealed class CollectionSequenceTests
	{
		[Fact]
		public void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.List.By.Self
			     .Out()
			     .Get(expected.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		public void VerifyAdvanced()
		{
			var source = Enumerable.Range(0, 10_000).ToList();
			Start.A.Selection<int>()
			     .As.Sequence.List.By.Self
			     .Select(x => x.Skip(3000)
			                   .Take(1000)
			                   .Where(y => y > 1000)
			                   .ToArray())
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(3000)
			                  .Take(1000)
			                  .Where(x => x > 1000)
			                  .ToArray());
		}

		[Fact]
		public void VerifyComprehensive()
		{
			var source = Enumerable.Range(0, 10_000).ToList();
			Start.A.Selection<int>()
			     .As.Sequence.List.By.Self
			     .Select(x => x.Skip(3000)
			                   .Take(2000)
			                   .Where(y => y > 1000)
			                   .Skip(500)
			                   .Take(1000)
			                   .ToArray())
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(3000)
			                  .Take(2000)
			                  .Where(x => x > 1000)
			                  .Skip(500)
			                  .Take(1000)
			                  .ToArray());
		}

		[Fact]
		public void VerifyCount()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Where(x => x > 1000).ToArray();
			var ints = Start.A.Selection<int>()
			                .As.Sequence.List.By.Self
			                .Select(x => x.Where(y => y > 1000).ToArray())
			                .Get(source.ToList())
				;
			ints.Should().NotBeSameAs(source);
			ints.Should().Equal(expected);
			ints.Should().HaveCountGreaterThan(5000);
		}

		[Fact]
		public void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.List.By.Self.Select(x => x.Skip(5000)
			                                            .Take(300)
			                                            .ToArray())
			     .Get(source.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		public void VerifyWhereLink()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.List.By.Self.Select(x => x.Where(y => y > 3).ToArray())
			     .Get(numbers.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		public void VerifyWhereSkipTake()
		{
			var       source = Enumerable.Range(0, 10_000).ToArray();
			const int count  = 8500;
			source.Where(x => x > 1000)
			      .Skip(count)
			      .Take(5)
			      .Should()
			      .Equal(Start.A.Selection<int>()
			                  .As.Sequence.List.By.Self
			                  .Select(x => x.Where(y => y > 1000)
			                                .Skip(count)
			                                .Take(5)
			                                .ToArray())
			                  .Get(source.ToList()));
		}

		[Fact]
		public void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual = Start.A.Selection<int>()
			                  .As.Sequence.List.By.Self
			                  .Select(x => x.Where(y => y > 3)
			                                .Take(1)
			                                .ToArray())
			                  .Get(numbers.ToList());
			actual.Should().Equal(expected);
			actual.Should().NotBeSameAs(numbers);
		}
	}
}