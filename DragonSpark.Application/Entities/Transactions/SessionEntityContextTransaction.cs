using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class SessionEntityContextTransaction : ITransaction
{
	readonly DbContext            _context;
	readonly IMutable<DbContext?> _store;

	public SessionEntityContextTransaction(DbContext context) : this(context, LogicalContext.Default) {}

	public SessionEntityContextTransaction(DbContext context, IMutable<DbContext?> store)
	{
		_context = context;
		_store   = store;
	}

	public void Execute(None parameter)
	{
		_store.Execute(_context);
	}

	public ValueTask Get() => default;

	public ValueTask DisposeAsync()
	{
		_store.Execute(default);
		return ValueTask.CompletedTask;
	}
}