using BenchmarkDotNet.Attributes;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

// ReSharper disable ImpureMethodCallOnReadonlyValueField

namespace DragonSpark.Testing.Model.Sequences
{
	public sealed class SessionTests
	{
		public class Benchmarks
		{
			readonly Session<object>
				_null  = new Session<object>(Empty<object>.Array, null),
				_empty = new Session<object>(Empty<object>.Array, EmptyCommand<object[]>.Default);

			[Benchmark]
			public void Empty() => _empty.Dispose();

			[Benchmark]
			public void Null() => _null.Dispose();
		}
	}
}