using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct StoredLease<T> : IDisposable, IAsyncDisposable
	{
		public static implicit operator T[](StoredLease<T> source) => source.Store.Elements;
		public static implicit operator Memory<T>(StoredLease<T> source) => source.Lease.AsMemory();

		public StoredLease(Lease<T> lease, Store<T> store)
		{
			Lease = lease;
			Store = store;
		}

		public Lease<T> Lease { get; }

		public Store<T> Store { get; }

		public void Dispose()
		{
			Lease.Dispose();
			Store.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			Dispose();
			return Task.CompletedTask.ToOperation();
		}
	}
}