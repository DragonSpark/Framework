using BenchmarkDotNet.Attributes;
using DragonSpark.Model.Sequences;
using DragonSpark.Testing.Objects;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Enumerable = System.Linq.Enumerable;

// ReSharper disable PossibleMultipleEnumeration

// ReSharper disable NotAccessedVariable
// ReSharper disable RedundantAssignment

namespace DragonSpark.Testing.Application
{
	public class ValueIterationBenchmarks
	{
		readonly IEnumerable<int> _sequence;
		readonly int[]            _open;

		readonly IEnumerable<int> _enumerable;

		public ValueIterationBenchmarks() : this(Enumerable.Range(0, 100)) {}

		public ValueIterationBenchmarks(IEnumerable<int> array) : this(array, array.ToArray()) {}

		public ValueIterationBenchmarks(IEnumerable<int> sequence, int[] open)
			: this(sequence, open, open) {}

		public ValueIterationBenchmarks(IEnumerable<int> sequence, int[] open, IEnumerable<int> enumerable)
		{
			_sequence   = sequence;
			_open       = open;
			_enumerable = enumerable;
		}

		[Benchmark(Baseline = true)]
		public void Array()
		{
			var array  = _open;
			var length = array.Length;
			for (var i = 0u; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark]
		public void Sequence()
		{
			foreach (var element in _sequence.AsValueEnumerable())
			{
				Call(element);
			}
		}

		[Benchmark]
		public void Open()
		{
			foreach (var element in _open.AsValueEnumerable())
			{
				Call(element);
			}
		}

		[Benchmark]
		public void OpenAsEnumerable()
		{
			foreach (var element in _enumerable.AsValueEnumerable())
			{
				Call(element);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		static void Call(int parameter) {}
	}

	public class IterationBenchmarks
	{
		readonly Array<string>          _array;
		readonly ImmutableArray<string> _immutable;
		readonly string[]               _open;

		public IterationBenchmarks() : this(Data.Default.Get().Take(100).ToArray()) {}

		public IterationBenchmarks(Array<string> array) : this(array, array, array) {}

		public IterationBenchmarks(ImmutableArray<string> immutable, Array<string> array, string[] open)
		{
			_immutable = immutable;
			_array     = array;
			_open      = open;
		}

		[Benchmark]
		public void Array()
		{
			var array  = _array;
			var length = array.Length;
			for (var i = 0u; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark]
		public void Immutable()
		{
			var array  = _immutable;
			var length = array.Length;
			for (var i = 0; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark(Baseline = true)]
		public void Open()
		{
			var open   = _open;
			var length = open.Length;
			for (var i = 0; i < length; i++)
			{
				Call(open[i]);
			}
		}

		[Benchmark]
		public void Span()
		{
			Span<string> array  = _open;
			var          length = array.Length;
			for (var i = 0; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark]
		public void Memory()
		{
			Memory<string> array     = _open;
			var            length    = array.Length;
			var            arraySpan = array.Span;
			for (var i = 0; i < length; i++)
			{
				Call(arraySpan[i]);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		static void Call(string parameter) {}
	}
}