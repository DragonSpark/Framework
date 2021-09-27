using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	sealed class DatabaseTransaction : ITransaction
	{
		readonly ITransaction          _previous;
		readonly IDbContextTransaction _transaction;

		public DatabaseTransaction(ITransaction previous, IDbContextTransaction transaction)
		{
			_previous    = previous;
			_transaction = transaction;
		}

		public async ValueTask Get()
		{
			await _previous.Await();
			await _transaction.CommitAsync().ConfigureAwait(false);
		}

		public async ValueTask DisposeAsync()
		{
			await _previous.DisposeAsync().ConfigureAwait(false);
			await _transaction.DisposeAsync().ConfigureAwait(false);
		}

		public void Execute(None parameter)
		{
			_previous.Execute(parameter);
		}
	}
}