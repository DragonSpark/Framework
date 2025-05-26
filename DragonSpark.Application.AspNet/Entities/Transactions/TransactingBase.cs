using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public class TransactingBase<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut>        _previous;
	readonly ITransactions                _transactions;
	readonly Func<TIn, CancellationToken> _stop;

	protected TransactingBase(ISelecting<TIn, TOut> previous, ITransactions transactions, Func<TIn, CancellationToken> stop)
	{
		_previous     = previous;
		_transactions = transactions;
		_stop         = stop;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var             stop        = _stop(parameter);
		await using var transaction = await _transactions.Off(stop);
		transaction.Execute();
		var result = await _previous.Off(parameter);
		await transaction.Off(stop);
		return result;
	}
}

public class TransactingBase<T> : IOperation<T>
{
	readonly IOperation<T>              _previous;
	readonly ITransactions              _transactions;
	readonly Func<T, CancellationToken> _stop;

	protected TransactingBase(IOperation<T> previous, ITransactions transactions, Func<T, CancellationToken> stop)
	{
		_previous     = previous;
		_transactions = transactions;
		_stop         = stop;
	}

	public async ValueTask Get(T parameter)
	{
		var             stop        = _stop(parameter);
		await using var transaction = await _transactions.Off(stop);
		transaction.Execute();
		await _previous.Off(parameter);
		await transaction.Off(stop);
	}
}