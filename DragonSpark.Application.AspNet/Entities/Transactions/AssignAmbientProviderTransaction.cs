using System;
using DragonSpark.Composition.Scopes;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class AssignAmbientProviderTransaction(IServiceProvider value)
    : StoreTransaction<IServiceProvider>(value, LogicalProvider.Default);
