using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

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
		await using var transaction = await _transactions.Off();
		transaction.Execute();
		await _previous.Off(parameter);
		await transaction.Off();
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
		await using var transaction = await _transactions.Off();
		transaction.Execute();
		var result = await _previous.Off(parameter);
		await transaction.Off();
		return result;
	}
}