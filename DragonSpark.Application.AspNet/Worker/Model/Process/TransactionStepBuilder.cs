using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class TransactionStepBuilder<T> : IStepBuilder<T> where T : ExternalProcess
{
	readonly IStepBuilder<T> _previous;
	readonly ITransactions   _transactions;

	protected TransactionStepBuilder(IStepBuilder<T> previous) : this(previous, ProcessTransactions.Default) {}

	protected TransactionStepBuilder(IStepBuilder<T> previous, ITransactions transactions)
	{
		_previous     = previous;
		_transactions = transactions;
	}

	public IStopAware<T> Get(Step<T> parameter)
	{
		var previous = _previous.Get(parameter);
		return new TransactionAwareStep<T>(previous, _transactions);
	}
}