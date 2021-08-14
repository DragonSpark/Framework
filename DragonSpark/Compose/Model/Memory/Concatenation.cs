using DragonSpark.Model.Sequences.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct Concatenation<T> : IDisposable, IAsyncDisposable
	{
		readonly Lease<T> _first;

		public Concatenation(Lease<T> first, Lease<T> instance)
		{
			_first   = first;
			Instance = instance;
		}

		public Lease<T> Result()
		{
			_first.Dispose();
			return Instance;
		}

		public Lease<T> Instance { get; }

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