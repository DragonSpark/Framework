using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class AmbientAwareEntityContextTransactions(IServiceScopedTransactions previous) : ITransactions
{
	readonly IServiceScopedTransactions _previous = previous;

	[MustDisposeResource]
	public ValueTask<ITransaction> Get()
	{
		var previous = _previous.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		return new(new AppendedTransaction(previous, new AssignAmbientEntityContextTransaction(context)));
	}
}
