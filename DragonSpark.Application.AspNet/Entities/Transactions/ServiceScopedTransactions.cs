using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class ServiceScopedTransactions : IServiceScopedTransactions
{
	readonly IMutable<AsyncServiceScope?> _scope;
	readonly IScoping                     _scoping;

	public ServiceScopedTransactions(IScoping scoping) : this(LogicalScope.Default, scoping) {}

	public ServiceScopedTransactions(IMutable<AsyncServiceScope?> scope, IScoping scoping)
	{
		_scope   = scope;
		_scoping = scoping;
	}

	public IScopedTransaction Get() => new ServiceScopedTransaction(_scope, _scoping.Get());
}