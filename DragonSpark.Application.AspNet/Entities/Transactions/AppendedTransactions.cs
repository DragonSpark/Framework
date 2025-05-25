using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

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
	public async ValueTask<ITransaction> Get(CancellationToken parameter)
		=> new AppendedTransaction(await _first.Off(parameter), await _second.Off(parameter));
}
