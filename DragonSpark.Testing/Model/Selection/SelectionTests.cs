using BenchmarkDotNet.Attributes;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Testing.Model.Selection
{
	public class SelectionTests
	{
		public class Benchmarks
		{
			readonly ISelect<string, string> _extensive;
			readonly ISelect<string, string> _multiple;
			readonly ISelect<string, string> _one;
			readonly ISelect<string, string> _subject;

			public Benchmarks() : this(A.Self<string>()) {}

			public Benchmarks(ISelect<string, string> subject)
				: this(subject, subject.Select(x => x),
				       subject.Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x),
				       subject.Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)
				              .Select(x => x)) {}

			// ReSharper disable once TooManyDependencies
			public Benchmarks(ISelect<string, string> subject, ISelect<string, string> one,
			                  ISelect<string, string> multiple, ISelect<string, string> extensive)
			{
				_subject   = subject;
				_one       = one;
				_multiple  = multiple;
				_extensive = extensive;
			}

			[Benchmark(Baseline = true)]
			public string Measure() => _subject.Get(string.Empty);

			[Benchmark]
			public string One() => _one.Get(string.Empty);

			[Benchmark]
			public string Multiple() => _multiple.Get(string.Empty);

			[Benchmark]
			public string Extensive() => _extensive.Get(string.Empty);
		}
	}
}