using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class ScopedEntityContextTransactions : ITransactions
{
	readonly IServiceProvider _first;
	readonly DbContext        _second;

	public ScopedEntityContextTransactions(IServiceProvider first, DbContext second)
	{
		_first  = first;
		_second = second;
	}

	public ValueTask<ITransaction> Get() => new SessionTransaction(_first, _second).ToOperation<ITransaction>();
}