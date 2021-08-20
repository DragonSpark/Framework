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
		readonly IAsyncDisposable _previous;

		public Query(DbContext context) : this(context, context.Set<T>()) {}

		public Query(IAsyncDisposable previous, IQueryable<T> subject)
		{
			_previous = previous;
			Subject   = subject;
		}

		public Query<TTo> Select<TTo>(IQueryable<TTo> parameter) where TTo : class => new(_previous, parameter);

		public IQueryable<T> Subject { get; }

		public ValueTask DisposeAsync() => _previous.DisposeAsync();

		public void Deconstruct(out IQueryable<T> subject)
		{
			subject = Subject;
		}
	}

	public class Root<TContext, T> : IQuery<T> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _contexts;

		protected Root(IDbContextFactory<TContext> contexts) => _contexts = contexts;

		public Query<T> Get() => new(_contexts.CreateDbContext());
	}




}