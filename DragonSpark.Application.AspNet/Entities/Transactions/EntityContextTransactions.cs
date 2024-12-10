using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class EntityContextTransactions(IServiceScopedTransactions previous) : ITransactions
{
	[MustDisposeResource]
	public ValueTask<ITransaction> Get()
	{
		var previous1 = previous.Get();
		var context   = previous1.Provider.GetRequiredService<DbContext>();
		var second    = new EntityContextTransaction(context);
		return new(new AppendedTransaction(previous1, second));
	}
}
