using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class Transacting<T> : IOperation<T>
	{
		readonly IOperation<T>  _previous;
		readonly DatabaseFacade _database;

		public Transacting(IOperation<T> previous, DatabaseFacade database)
		{
			_previous = previous;
			_database = database;
		}

		public async ValueTask Get(T parameter)
		{
			await using var transaction = await _database.BeginTransactionAsync().ConfigureAwait(false);
			await _previous.Await(parameter);
			await transaction.CommitAsync().ConfigureAwait(false);
		}
	}

	public class Transacting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut> _previous;
		readonly DatabaseFacade        _database;

		public Transacting(ISelecting<TIn, TOut> previous, DatabaseFacade database)
		{
			_previous = previous;
			_database = database;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			await using var transaction = await _database.BeginTransactionAsync().ConfigureAwait(false);
			var             result      = await _previous.Await(parameter);
			await transaction.CommitAsync().ConfigureAwait(false);
			return result;
		}
	}

}