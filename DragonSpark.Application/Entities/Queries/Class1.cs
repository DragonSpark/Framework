using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IQuery<T> : IResult<Query<T>> where T : class {}

	public readonly struct Query<T> : IAsyncDisposable where T : class
	{
		readonly DbContext _context;


		public Query(DbContext context) : this(context, context.Set<T>()) {}

		public Query(DbContext context, IQueryable<T> subject)
		{
			_context = context;
			Subject  = subject;
		}

		public Query<TTo> Select<TTo>(IQueryable<TTo> parameter) where TTo : class => new(_context, parameter);

		public IQueryable<T> Subject { get; }

		public ValueTask DisposeAsync() => _context.DisposeAsync();

		public void Deconstruct(out DbContext context, out DbSet<T> subject)
		{
			context = _context;
			subject = _context.Set<T>();
		}
	}

	public class Root<TContext, T> : IQuery<T> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _contexts;

		protected Root(IDbContextFactory<TContext> contexts) => _contexts = contexts;

		public Query<T> Get() => new(_contexts.CreateDbContext());
	}
}