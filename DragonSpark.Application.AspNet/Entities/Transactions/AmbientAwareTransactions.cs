using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class AmbientAwareTransactions : ITransactions
{
	readonly ITransactions _previous;

	public AmbientAwareTransactions(ITransactions previous) => _previous = previous;

	public async ValueTask<ITransaction> Get()
	{
		var previous = await _previous.Await();
		return new AmbientAwareTransaction(previous);
	}
}