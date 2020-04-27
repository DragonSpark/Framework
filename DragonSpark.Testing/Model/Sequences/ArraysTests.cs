using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using System.Linq;
using Xunit;
using Array = System.Array;

namespace DragonSpark.Testing.Model.Sequences
{
	public sealed class ArraysTests
	{
		public class Benchmarks
		{
			readonly uint[]                       _source;
			readonly ISelect<Store<uint>, uint[]> _sut;

			public Benchmarks() : this(Near.Default) {}

			public Benchmarks(DragonSpark.Model.Sequences.Selection selection)
				: this(new Arrays<uint>(selection),
				       FixtureInstance.Default
				                      .Many<uint>(10_000)
				                      .Get()
				                      .ToArray()) {}

			public Benchmarks(ISelect<Store<uint>, uint[]> sut, uint[] source)
			{
				_sut    = sut;
				_source = source;
			}

			[Benchmark]
			public Array Measure() => _sut.Get(_source);
		}

		[Fact]
		public void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Arrays<int>.Default.Get(expected)
			           .Should()
			           .Equal(expected);
		}

		[Fact]
		public void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			Arrays<int>.Default.Get(expected)
			           .Should()
			           .Equal(expected);
		}
	}
}