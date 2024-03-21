using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class AssignAmbientEntityContextTransaction : StoreTransaction<DbContext>
{
	public AssignAmbientEntityContextTransaction(DbContext context) : base(context, LogicalContext.Default) {}
}

// TODO

sealed class DisposingTransaction : ITransaction
{
	readonly IDisposable _disposable;

	public DisposingTransaction(IDisposable disposable) => _disposable = disposable;

	public async ValueTask DisposeAsync()
	{
		if (_disposable is IAsyncDisposable disposableAsyncDisposable)
		{
			await disposableAsyncDisposable.DisposeAsync().ConfigureAwait(false);
		}
		else
		{
			_disposable.Dispose();
		}
	}

	public void Execute(None parameter) {}

	public ValueTask Get() => ValueTask.CompletedTask;
}