using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

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
	public ValueTask<ITransaction> Get(CancellationToken parameter)
		=> new(new AssignAmbientComponentsTransaction(_first, _second));
}