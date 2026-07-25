using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class ProcessTransactions : ITransactions
{
	public static ProcessTransactions Default { get; } = new();

	ProcessTransactions() : this(LogicalDatabaseTransactions.Default) {}

	readonly ITransactions _previous;

	public ProcessTransactions(ITransactions previous) => _previous = previous;

	public async ValueTask<ITransaction> Get(CancellationToken parameter)
		=> new ProcessTransaction(await _previous.Off(parameter));
}