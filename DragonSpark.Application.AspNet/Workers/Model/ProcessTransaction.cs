using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class ProcessTransaction : ITransaction, IContextAware
{
	readonly ITransaction _previous;
	readonly DbContext    _context;

	public ProcessTransaction(ITransaction previous)
		: this(new AmbientAwareTransaction(previous), previous.To<IContextAware>().Get()) {}

	public ProcessTransaction(ITransaction previous, DbContext context)
	{
		_previous = previous;
		_context  = context;
	}

	public void Execute(None parameter)
	{
		_previous.Execute(parameter);
	}

	public ValueTask Get(CancellationToken parameter) => _previous.Get(parameter);

	public ValueTask DisposeAsync() => _previous.DisposeAsync();

	DbContext IResult<DbContext>.Get() => _context;
}