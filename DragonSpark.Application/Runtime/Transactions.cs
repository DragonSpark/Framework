using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime
{
	public readonly struct Transactions<T> : IDisposable, IAsyncDisposable
	{
		public Transactions(Lease<T> add, Lease<(T Stored, T Source)> update, Lease<T> delete)
		{
			Add    = add;
			Update = update;
			Delete = delete;
		}

		public Lease<T> Add { get; }

		public Lease<(T Stored, T Source)> Update { get; }

		public Lease<T> Delete { get; }

		public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;

		public TransactionSpans<T> AsSpans() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());

		public void Deconstruct(out Memory<T> add, out Memory<(T Stored, T Source)> update, out Memory<T> delete)
		{
			add    = Add.AsMemory();
			update = Update.AsMemory();
			delete = Delete.AsMemory();
		}

		public void Dispose()
		{
			Add.Dispose();
			Update.Dispose();
			Delete.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			Dispose();
			return Task.CompletedTask.ToOperation();
		}
	}

}