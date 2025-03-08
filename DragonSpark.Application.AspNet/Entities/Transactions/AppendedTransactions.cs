using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public class AppendedTransactions : ITransactions
{
	readonly ITransactions _first;
	readonly ITransactions _second;

	protected AppendedTransactions(ITransactions first, ITransactions second)
	{
		_first  = first;
		_second = second;
	}

	[MustDisposeResource]
	public async ValueTask<ITransaction> Get() => new AppendedTransaction(await _first.Off(), await _second.Off());
}
