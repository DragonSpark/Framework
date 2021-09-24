using DragonSpark.Model.Sequences.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct Concatenation<T> : IDisposable, IAsyncDisposable
	{
		readonly Leasing<T> _first;

		public Concatenation(Leasing<T> first, Leasing<T> instance)
		{
			_first   = first;
			Instance = instance;
		}

		public Leasing<T> Result()
		{
			_first.Dispose();
			return Instance;
		}

		public Leasing<T> Instance { get; }

		public void Dispose()
		{
			var first = _first;
			first.Dispose();

			var result = Instance;
			result.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			Dispose();
			return Task.CompletedTask.ToOperation();
		}
	}
}