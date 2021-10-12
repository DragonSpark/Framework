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

		public class InParameterSelectionBenchmarks
		{
			readonly ISelect<Reference, Reference>   _reference;
			readonly ISelect<Writable, Writable>     _writable;
			readonly ISelect<Readable, Readable>     _readable;
			readonly IInSelect<Reference, Reference> _referenceIn;
			readonly IInSelect<Writable, Writable>   _writableIn;
			readonly IInSelect<Readable, Readable>   _readableIn;

			public InParameterSelectionBenchmarks()
				: this(A.Self<Reference>()
						.Select(x => x)
						.Select(x => x)
						.Select(x => x)
						.Select(x => x),
				       A.Self<Writable>()
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x),
				       A.Self<Readable>()
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x)
				        .Select(x => x),
					  new InSelect<Reference, Reference, Reference>(
							new InSelect<Reference, Reference, Reference>(
							  new InSelect<Reference, Reference, Reference>(
								new InSelect<Reference, Reference, Reference>(
									(in Reference x) => x, (in Reference x) => x).Get,
								(in Reference x) => x).Get,
							  (in Reference x) => x).Get,
							(in Reference x) => x),
					   new InSelect<Writable, Writable, Writable>(
						   new InSelect<Writable, Writable, Writable>(
							   new InSelect<Writable, Writable, Writable>(
								   new InSelect<Writable, Writable, Writable>(
										(in Writable x) => x, (in Writable x) => x).Get,
								   (in Writable x) => x).Get,
							   (in Writable x) => x).Get,
						   (in Writable x) => x),
					   new InSelect<Readable, Readable, Readable>(
						   new InSelect<Readable, Readable, Readable>(
							   new InSelect<Readable, Readable, Readable>(
								   new InSelect<Readable, Readable, Readable>((in Readable x) => x,
									   (in Readable x) => x).Get,
								   (in Readable x) => x).Get,
							   (in Readable x) => x).Get,
						   (in Readable x) => x)) {}

			// ReSharper disable once TooManyDependencies
			public InParameterSelectionBenchmarks(ISelect<Reference, Reference> reference,
			                                    ISelect<Writable, Writable> writable,
			                                    ISelect<Readable, Readable> readable,
			                                    IInSelect<Reference, Reference> referenceIn,
			                                    IInSelect<Writable, Writable> writableIn,
			                                    IInSelect<Readable, Readable> readableIn)
			{
				_reference   = reference;
				_writable    = writable;
				_readable    = readable;
				_referenceIn = referenceIn;
				_writableIn  = writableIn;
				_readableIn  = readableIn;
			}

			[Benchmark(Baseline = true)]
			public object ReferenceSelect() => _reference.Get(new Reference(new object(), new object(), new object())).First;

			[Benchmark]
			public object WriteSelect() => _writable.Get(new Writable(new object(), new object(), new object())).Message;

			[Benchmark]
			public object ReadSelect() => _readable.Get(new Readable(new object(), new object(), new object())).Message;

			[Benchmark]
			public object ClassIn() => _referenceIn.Get(new Reference(new object(), new object(), new object())).First;

			[Benchmark]
			public object WriteIn() => _writableIn.Get(new Writable(new object(), new object(), new object())).Message;

			[Benchmark]
			public object ReadIn() => _readableIn.Get(new Readable(new object(), new object(), new object())).Message;
		}

		public sealed class Reference
		{
			public object First { get; }
			public object Second { get; }
			public object Third { get; }

			public Reference(object first, object second, object third)
			{
				First  = first;
				Second = second;
				Third  = third;
			}
		}

		public readonly struct Readable
		{
			public object Message { get; }
			public object Other { get; }
			public object Three { get; }

			public Readable(object message, object other, object three)
			{
				Message = message;
				Other   = other;
				Three   = three;
			}
		}

		public struct Writable
		{
			public object Message { get; }
			public object Other { get; }
			public object Three { get; }

			public Writable(object message, object other, object three)
			{
				Message = message;
				Other   = other;
				Three   = three;
			}
		}
	}

	public class InSelect<TIn, TFrom, TTo> : IInSelect<TIn, TTo>
	{
		readonly In<TFrom, TTo> _current;
		readonly In<TIn, TFrom> _previous;

		public InSelect(IInSelect<TIn, TFrom> previous, IInSelect<TFrom, TTo> current)
			: this(previous.Get, current.Get) {}

		public InSelect(In<TIn, TFrom> previous, In<TFrom, TTo> current)
		{
			_previous = previous;
			_current  = current;
		}

		public TTo Get(in TIn parameter) => _current(_previous(in parameter));
	}
}