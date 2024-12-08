using System;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class ServiceScopedDatabaseTransaction : AppendedTransaction, IScopedTransaction
{
	public ServiceScopedDatabaseTransaction(IScopedTransaction first, ITransaction second)
		: this(first.Provider, first, second) {}

	public ServiceScopedDatabaseTransaction(IServiceProvider provider, ITransaction first, ITransaction second)
		: base(first, second)
		=> Provider = provider;

	public IServiceProvider Provider { get; }
}