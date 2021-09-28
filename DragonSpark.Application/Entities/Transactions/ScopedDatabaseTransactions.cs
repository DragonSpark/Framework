using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	public sealed class ScopedDatabaseTransactions : ITransactions
	{
		readonly IScopedTransactions _boundaries;

		public ScopedDatabaseTransactions(IScopedTransactions boundaries) => _boundaries = boundaries;

		public async ValueTask<ITransaction> Get()
		{
			var previous = _boundaries.Get();
			var transaction = await previous.Provider.GetRequiredService<DbContext>()
			                                .Database.BeginTransactionAsync()
			                                .ConfigureAwait(false);
			var result = new AppendedTransaction(previous, new DatabaseTransaction(transaction));
			return result;
		}
	}
}