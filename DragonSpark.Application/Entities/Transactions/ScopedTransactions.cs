using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class ScopedTransactions : IScopedTransactions
{
	readonly IScoping                     _scoping;
	readonly IMutable<AsyncServiceScope?> _store;

	public ScopedTransactions(IScoping scoping) : this(scoping, LogicalScope.Default) {}

	public ScopedTransactions(IScoping scoping, IMutable<AsyncServiceScope?> store)
	{
		_scoping = scoping;
		_store   = store;
	}

	public IScopedTransaction Get() => new ServiceScopedTransaction(_store, _scoping.Get());
}