using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class EntityContextTransaction(DbContext context) : ITransaction
{
	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await context.SaveChangesAsync().Await();
	}

	public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
