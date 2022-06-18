using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class SessionBoundaryTransactions : ITransactions
{
	readonly IBoundary              _boundary;
	readonly IMutable<IDisposable?> _store;

	public SessionBoundaryTransactions(InstanceBoundary boundary) : this(boundary, AmbientLock.Default) {}

	public SessionBoundaryTransactions(IBoundary boundary, IMutable<IDisposable?> store)
	{
		_boundary = boundary;
		_store    = store;
	}

	public async ValueTask<ITransaction> Get() => new SessionInstanceTransaction(await _boundary.Await(), _store);
}