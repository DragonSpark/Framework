using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class ServiceScopedTransaction : IScopedTransaction
{
	readonly ICommand<AsyncServiceScope?> _store;
	readonly AsyncServiceScope            _instance;

	public ServiceScopedTransaction(ICommand<AsyncServiceScope?> store, AsyncServiceScope instance)
		: this(store, instance, instance.ServiceProvider) {}

	public ServiceScopedTransaction(ICommand<AsyncServiceScope?> store, AsyncServiceScope instance,
	                                IServiceProvider provider)
	{
		Provider  = provider;
		_store    = store;
		_instance = instance;
	}

	public IServiceProvider Provider { get; }

	public ValueTask Get() => ValueTask.CompletedTask;

	public void Execute(None parameter)
	{
		_store.Execute(_instance);
	}

	public ValueTask DisposeAsync()
	{
		_store.Execute(default);
		return _instance.DisposeAsync();
	}
}