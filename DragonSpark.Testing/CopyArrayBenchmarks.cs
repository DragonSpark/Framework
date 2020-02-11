using AutoFixture;
using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

namespace DragonSpark.Testing
{
	public class CopyArrayBenchmarks<T>
	{
		const uint Total = 10_000u;

		readonly T[] _source = new Fixture().CreateMany<T>((int)Total).ToArray();

		[Params(Total)]
		public uint Count { get; set; }

		[Benchmark]
		public Array CopyTo()
		{
			var result = new T[Count];
			_source.CopyTo(result, 0);
			return result;
		}

		[Benchmark]
		public Array Span()
		{
			var result = new T[Count];
			_source.CopyTo(result.AsSpan());
			return result;
		}

		[Benchmark]
		public Array Memory()
		{
			var result = new T[Count];
			_source.CopyTo(result.AsMemory());
			return result;
		}

		[Benchmark]
		public Array ToArray() => _source.ToArray();

		[Benchmark]
		public Array Copy()
		{
			var result = new T[Count];
			Array.Copy(_source, 0, result, 0, _source.Length);
			return result;
		}
	}
}