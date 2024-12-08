using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class AssignAmbientEntityContextTransaction : StoreTransaction<DbContext>
{
	public AssignAmbientEntityContextTransaction(DbContext context) : base(context, LogicalContext.Default) {}
}