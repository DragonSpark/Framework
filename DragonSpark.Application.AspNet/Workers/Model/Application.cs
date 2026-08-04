using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class Application<T> : IStopAware<Guid> where T : ExternalProcess
{
	readonly ITransactions       _transactions;
	readonly IStopAware<Guid, T> _select;
	readonly IStopAware<T>       _process;

	protected Application(AmbientAwareEntityContextTransactions transactions, IStopAware<Guid, T> select,
	                      IStopAware<T> process)
	{
		_transactions = transactions;
		_select       = select;
		_process      = process;
	}

	public async ValueTask Get(Stop<Guid> parameter)
	{
		await using var transaction = await _transactions.Off(parameter);
		transaction.Execute();
		var subject = await _select.Off(parameter);
		await _process.Off(new(subject, parameter));
		await transaction.Off(parameter);
	}
}