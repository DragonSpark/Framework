using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	public class SessionDatabaseTransactions : ITransactions
	{
		readonly DatabaseFacade _facade;

		public SessionDatabaseTransactions(DbContext owner) : this(owner.Database) {}

		public SessionDatabaseTransactions(DatabaseFacade facade) => _facade = facade;

		public async ValueTask<ITransaction> Get()
		{
			var transaction = await _facade.BeginTransactionAsync().ConfigureAwait(false);
			return new DatabaseTransaction(transaction);
		}
	}
}