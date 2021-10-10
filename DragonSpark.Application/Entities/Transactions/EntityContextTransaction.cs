using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	sealed class EntityContextTransaction : ITransaction
	{
		readonly DbContext _context;

		public EntityContextTransaction(DbContext context) => _context = context;

		public void Execute(None parameter) {}

		public async ValueTask Get()
		{
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public ValueTask DisposeAsync() => ValueTask.CompletedTask;
	}

	public sealed class EntityContextTransactions : ITransactions
	{
		readonly IScopedTransactions _boundaries;

		public EntityContextTransactions(IScopedTransactions boundaries) => _boundaries = boundaries;

		public ValueTask<ITransaction> Get()
		{
			var previous = _boundaries.Get();
			var context  = previous.Provider.GetRequiredService<DbContext>();
			var second   = new EntityContextTransaction(context);
			var result   = new AppendedTransaction(previous, second).ToOperation<ITransaction>();
			return result;
		}
	}
}