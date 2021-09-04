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

		public class StructureSelectionBenchmarks
		{
			readonly ISelect<Reference, Reference> _reference;
			readonly ISelect<Writable, Writable>   _writable;
			readonly ISelect<Readable, Readable>   _readable;

			public StructureSelectionBenchmarks()
				: this(A.Self<Reference>()
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x),
				       A.Self<Writable>()
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x),
				       A.Self<Readable>()
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)) {}

			public StructureSelectionBenchmarks(ISelect<Reference, Reference> reference, ISelect<Writable, Writable> writable,
			                                    ISelect<Readable, Readable> readable)
			{
				_reference = reference;
				_writable  = writable;
				_readable  = readable;
			}

			[Benchmark(Baseline = true)]
			public string Class() => _reference.Get(new Reference("One", "Two", "Three")).First;

			[Benchmark]
			public string Write() => _writable.Get(new Writable("One", "Two", "Three")).Message;

			[Benchmark]
			public string Read() => _readable.Get(new Readable("One", "Two", "Three")).Message;
		}

		public sealed class Reference
		{
			public string First { get; }
			public string Second { get; }
			public string Third { get; }

			public Reference(string first, string second, string third)
			{
				First      = first;
				Second     = second;
				Third = third;
			}
		}

		public readonly struct Readable
		{
			public string Message { get; }
			public string Other { get; }
			public string Three { get; }

			public Readable(string message, string other, string three)
			{
				Message    = message;
				Other      = other;
				Three = three;
			}
		}

		public struct Writable
		{
			public string Message { get; }
			public string Other { get; }
			public string Three { get; }

			public Writable(string message, string other, string three)
			{
				Message    = message;
				Other      = other;
				Three = three;
			}
		}
	}
}