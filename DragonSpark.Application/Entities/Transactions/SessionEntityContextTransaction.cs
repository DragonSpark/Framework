using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Transactions;

sealed class SessionEntityContextTransaction : StoreTransaction<DbContext>
{
	public SessionEntityContextTransaction(DbContext context) : base(context, LogicalContext.Default) {}
}