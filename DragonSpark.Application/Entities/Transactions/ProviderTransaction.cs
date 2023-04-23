using DragonSpark.Composition.Scopes;
using System;

namespace DragonSpark.Application.Entities.Transactions;

sealed class ProviderTransaction : StoreTransaction<IServiceProvider>
{
	public ProviderTransaction(IServiceProvider value) : base(value, LogicalProvider.Default) {}
}