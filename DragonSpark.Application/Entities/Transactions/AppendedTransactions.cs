using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public class AppendedTransactions : ITransactions
{
	readonly ITransactions _first;
	readonly ITransactions _second;

	protected AppendedTransactions(ITransactions first, ITransactions second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<ITransaction> Get()
		=> new AppendedTransaction(await _first.Await(), await _second.Await());
}