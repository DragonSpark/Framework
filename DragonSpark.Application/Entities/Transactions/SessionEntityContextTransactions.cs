using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions
{
	public sealed class SessionEntityContextTransactions : ITransactions
	{
		readonly DbContext _session;

		public SessionEntityContextTransactions(DbContext session) => _session = session;

		public ValueTask<ITransaction> Get()
			=> new SessionEntityContextTransaction(_session).ToOperation<ITransaction>();
	}
}