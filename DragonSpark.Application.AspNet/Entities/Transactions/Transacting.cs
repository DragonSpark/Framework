using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public class Transacting<TIn, TOut> : TransactingBase<Stop<TIn>, TOut>
{
	protected Transacting(ISelecting<Stop<TIn>, TOut> previous, ITransactions transactions)
		: base(previous, transactions, x => x.Token) {}
}

public class Transacting<T> : TransactingBase<Stop<T>>
{
	protected Transacting(IOperation<Stop<T>> previous, ITransactions transactions)
		: base(previous, transactions, x => x.Token) {}
}