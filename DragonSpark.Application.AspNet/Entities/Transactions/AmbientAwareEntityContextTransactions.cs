using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class AmbientAwareEntityContextTransactions(IServiceScopedTransactions previous) : ITransactions
{
	readonly IServiceScopedTransactions _previous = previous;

	[MustDisposeResource]
	public ValueTask<ITransaction> Get(CancellationToken parameter)
	{
		var previous = _previous.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		return new(new AppendedTransaction(previous, new AssignAmbientEntityContextTransaction(context)));
	}
}
