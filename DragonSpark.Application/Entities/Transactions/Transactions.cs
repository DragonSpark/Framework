using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class Transactions : ITransactions
{
	readonly IScopedTransactions _transactions;

	public Transactions(IScopedTransactions transactions) => _transactions = transactions;

	public ValueTask<ITransaction> Get() => _transactions.Get().ToOperation<ITransaction>();
}