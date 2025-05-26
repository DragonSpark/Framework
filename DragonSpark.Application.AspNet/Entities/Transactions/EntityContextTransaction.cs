using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class EntityContextTransaction(DbContext context) : ITransaction
{
	public void Execute(None parameter) {}

	public async ValueTask Get(CancellationToken parameter)
	{
		await context.SaveChangesAsync(parameter).Off();
	}

	public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
