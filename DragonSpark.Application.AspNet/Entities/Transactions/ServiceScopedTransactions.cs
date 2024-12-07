using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities.Transactions;

sealed class ServiceScopedTransactions : IServiceScopedTransactions
{
	readonly IScoping                     _scoping;
	readonly IMutable<AsyncServiceScope?> _store;

	public ServiceScopedTransactions(IScoping scoping) : this(scoping, LogicalScope.Default) {}

	public ServiceScopedTransactions(IScoping scoping, IMutable<AsyncServiceScope?> store)
	{
		_scoping = scoping;
		_store   = store;
	}

	public IScopedTransaction Get() => new ServiceScopedTransaction(_store, _scoping.Get());
}