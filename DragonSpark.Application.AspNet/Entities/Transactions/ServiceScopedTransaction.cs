using DragonSpark.Model;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class ServiceScopedTransaction(
	ICommand<AsyncServiceScope?> store,
	AsyncServiceScope instance,
	IServiceProvider provider)
	: IScopedTransaction
{
	public ServiceScopedTransaction(ICommand<AsyncServiceScope?> store, AsyncServiceScope instance)
		: this(store, instance, instance.ServiceProvider) {}

	public IServiceProvider Provider { get; } = provider;

	public ValueTask Get(CancellationToken _) => ValueTask.CompletedTask;

	public void Execute(None parameter)
	{
		store.Execute(instance);
	}

	public ValueTask DisposeAsync()
	{
		store.Execute(null);
		return instance.DisposeAsync();
	}
}
