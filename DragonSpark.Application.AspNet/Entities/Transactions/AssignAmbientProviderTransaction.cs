using DragonSpark.Composition.Scopes;
using System;

namespace DragonSpark.Application.Entities.Transactions;

sealed class AssignAmbientProviderTransaction : StoreTransaction<IServiceProvider>
{
	public AssignAmbientProviderTransaction(IServiceProvider value) : base(value, LogicalProvider.Default) {}
}