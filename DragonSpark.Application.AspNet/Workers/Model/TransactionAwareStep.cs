using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class TransactionAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous;
	readonly ITransactions _transactions;

	public TransactionAwareStep(IStopAware<T> previous, ITransactions transactions)
	{
		_previous     = previous;
		_transactions = transactions;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await using var transaction = await _transactions.Off(parameter);
		transaction.Execute();
		transaction.To<IContextAware>().Get().Attach(parameter.Subject);
		try
		{
			await _previous.Off(parameter);
			await transaction.Off(parameter);
		}
		catch (AbortProcessException)
		{
			await transaction.Off(parameter);
			throw;
		}
	}
}