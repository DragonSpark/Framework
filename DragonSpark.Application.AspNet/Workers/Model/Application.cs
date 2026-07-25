using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class Application<T> : IStopAware<Guid> where T : ExternalProcess
{
	readonly ITransactions    _transactions;
	readonly IStopAware<Guid> _body;

	protected Application(AmbientAwareEntityContextTransactions transactions, IStopAware<Guid, T> select,
	                      IStopAware<T> process)
		: this(transactions, select.Then().Terminate(process).Out()) {}

	protected Application(ITransactions transactions, IStopAware<Guid> body)
	{
		_transactions = transactions;
		_body         = body;
	}

	public async ValueTask Get(Stop<Guid> parameter)
	{
		await using var transaction = await _transactions.Off(parameter);
		transaction.Execute();
		await _body.Off(parameter);
		await transaction.Off(parameter);
	}
}