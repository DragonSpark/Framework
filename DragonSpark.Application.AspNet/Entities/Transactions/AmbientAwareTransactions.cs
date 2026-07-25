using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class AmbientAwareTransactions : ITransactions
{
	readonly ITransactions _previous;

	public AmbientAwareTransactions(ITransactions previous) => _previous = previous;

	[MustDisposeResource]
	public async ValueTask<ITransaction> Get(CancellationToken parameter)
	{
		var previous = await _previous.Off(parameter);
		return new AmbientAwareTransaction(previous);
	}
}
