using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Linq;
using Enumerable = System.Linq.Enumerable;

namespace DragonSpark.Testing
{
	// ReSharper disable PossibleMultipleEnumeration
	public class ValueIterationBenchmarks
	{
		// ReSharper disable once UnusedParameter.Local
		static void Call(int parameter) {}

		readonly IEnumerable<int> _enumerable;
		readonly int[]            _open;
		readonly IEnumerable<int> _sequence;

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
	}
}