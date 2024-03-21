using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class Transactions : ITransactions
{
	readonly IServiceScopedTransactions _transactions;

	public Transactions(IServiceScopedTransactions transactions) => _transactions = transactions;

	public ValueTask<ITransaction> Get() => _transactions.Get().ToOperation<ITransaction>();
}