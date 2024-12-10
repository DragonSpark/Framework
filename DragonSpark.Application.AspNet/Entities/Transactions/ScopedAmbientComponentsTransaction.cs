using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class ScopedAmbientComponentsTransaction : ITransactions
{
	readonly IServiceProvider _first;
	readonly DbContext        _second;

	public ScopedAmbientComponentsTransaction(IServiceProvider first, DbContext second)
	{
		_first  = first;
		_second = second;
	}

	[MustDisposeResource]
	public ValueTask<ITransaction> Get() => new(new AssignAmbientComponentsTransaction(_first, _second));
}
