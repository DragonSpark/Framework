using BenchmarkDotNet.Attributes;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Linq;
using Enumerable = NetFabric.Hyperlinq.Enumerable;

namespace DragonSpark.Testing.Application
{
	// ReSharper disable PossibleMultipleEnumeration
	public class ValueIterationBenchmarks
	{
		readonly IEnumerable<int> _sequence;
		readonly int[]            _open;

		readonly IEnumerable<int> _enumerable;

		public ValueIterationBenchmarks() : this(System.Linq.Enumerable.Range(0, 100)) {}


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
			foreach (var element in Enumerable.AsValueEnumerable(_sequence))
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
}