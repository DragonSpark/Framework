using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Transactions;

sealed class SessionTransaction : AppendedTransaction
{
	public SessionTransaction(IServiceProvider first, DbContext second)
		: base(new ProviderTransaction(first), new SessionEntityContextTransaction(second)) {}
}