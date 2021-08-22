using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	public class SaveMany<TContext, T> : IOperation<Array<T>> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _context;

		public SaveMany(IDbContextFactory<TContext> context) => _context = context;

		public async ValueTask Get(Array<T> parameter)
		{
			await using var context = _context.CreateDbContext();
			context.Set<T>().UpdateRange(parameter.Open());
			await context.SaveChangesAsync();
		}
	}
}