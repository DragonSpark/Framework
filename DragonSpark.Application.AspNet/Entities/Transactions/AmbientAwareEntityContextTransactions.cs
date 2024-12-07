using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class AmbientAwareEntityContextTransactions : ITransactions
{
	readonly IServiceScopedTransactions _previous;

	public AmbientAwareEntityContextTransactions(IServiceScopedTransactions previous) => _previous = previous;

	public ValueTask<ITransaction> Get()
	{
		var previous = _previous.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		var appended = new AppendedTransaction(previous, new AssignAmbientEntityContextTransaction(context));
		return appended.ToOperation<ITransaction>();
	}
}