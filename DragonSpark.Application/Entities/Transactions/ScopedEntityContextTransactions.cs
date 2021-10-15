using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class ScopedEntityContextTransactions : ITransactions
{
	readonly DbContext _scoped;

	public ScopedEntityContextTransactions(DbContext scoped) => _scoped = scoped;

	public ValueTask<ITransaction> Get()
		=> new SessionEntityContextTransaction(_scoped).ToOperation<ITransaction>();
}