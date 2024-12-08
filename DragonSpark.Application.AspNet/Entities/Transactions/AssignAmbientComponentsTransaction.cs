using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

sealed class AssignAmbientComponentsTransaction : AppendedTransaction
{
	public AssignAmbientComponentsTransaction(IServiceProvider first, DbContext second)
		: base(new AssignAmbientProviderTransaction(first), new AssignAmbientEntityContextTransaction(second)) {}
}