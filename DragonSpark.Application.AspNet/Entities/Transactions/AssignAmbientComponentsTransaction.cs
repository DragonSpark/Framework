using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class AssignAmbientComponentsTransaction : AppendedTransaction
{
	public AssignAmbientComponentsTransaction(IServiceProvider first, DbContext second)
		: base(new AssignAmbientProviderTransaction(first), new AssignAmbientEntityContextTransaction(second)) {}
}