using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	sealed class DatabaseTransaction : ITransaction
	{
		readonly IDbContextTransaction _transaction;

		public DatabaseTransaction(IDbContextTransaction transaction) => _transaction = transaction;

		public void Execute(None parameter) {}

		public ValueTask Get() => _transaction.CommitAsync().ToOperation();

		public ValueTask DisposeAsync() => _transaction.DisposeAsync();
	}
}