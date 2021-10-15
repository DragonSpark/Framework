using DragonSpark.Model;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class SessionInstanceTransaction : ITransaction
{
	readonly IDisposable            _instance;
	readonly IMutable<IDisposable?> _store;

	public SessionInstanceTransaction(IDisposable instance) : this(instance, AmbientLock.Default) {}

	public SessionInstanceTransaction(IDisposable instance, IMutable<IDisposable?> store)
	{
		_instance = instance;
		_store    = store;
	}

	public void Execute(None parameter)
	{
		_store.Execute(_instance);
	}

	public ValueTask Get() => ValueTask.CompletedTask;

	public ValueTask DisposeAsync()
	{
		_instance.Dispose();
		_store.Execute(default);
		return ValueTask.CompletedTask;
	}
}