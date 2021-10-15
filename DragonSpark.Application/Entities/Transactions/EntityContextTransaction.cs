using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

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