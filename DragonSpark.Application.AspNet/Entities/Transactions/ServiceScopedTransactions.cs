using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class ServiceScopedTransactions(IMutable<AsyncServiceScope?> scope, IScoping scoping)
	: IServiceScopedTransactions
{
	public ServiceScopedTransactions(IScoping scoping) : this(LogicalScope.Default, scoping) {}

	[MustDisposeResource]
	public IScopedTransaction Get() => new ServiceScopedTransaction(scope, scoping.Get());
}
