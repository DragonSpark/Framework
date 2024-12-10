using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class AssignAmbientEntityContextTransaction(DbContext context)
	: StoreTransaction<DbContext>(context, LogicalContext.Default);
