using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public class Transacting<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly ITransactions _transactions;

	protected Transacting(IOperation<T> previous, ITransactions transactions)
	{
		_previous     = previous;
		_transactions = transactions;
	}

	public async ValueTask Get(T parameter)
	{
		await using var transaction = await _transactions.Await();
		transaction.Execute();
		await _previous.Await(parameter);
		await transaction.Await();
	}
}

public class Transacting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly ITransactions         _transactions;

	protected Transacting(ISelecting<TIn, TOut> previous, ITransactions transactions)
	{
		_previous     = previous;
		_transactions = transactions;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		await using var transaction = await _transactions.Await();
		transaction.Execute();
		var result = await _previous.Await(parameter);
		await transaction.Await();
		return result;
	}
}