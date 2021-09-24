using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct StoredLease<T> : IDisposable, IAsyncDisposable
	{
		public static implicit operator T[](StoredLease<T> source) => source.Store.Elements;
		public static implicit operator Memory<T>(StoredLease<T> source) => source.Leasing.AsMemory();

		public StoredLease(Leasing<T> memory, Store<T> store)
		{
			Leasing = memory;
			Store = store;
		}

		public Leasing<T> Leasing { get; }

		public Store<T> Store { get; }

		public void Dispose()
		{
			Leasing.Dispose();
			Store.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			Dispose();
			return Task.CompletedTask.ToOperation();
		}
	}
}