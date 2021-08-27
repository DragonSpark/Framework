using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IContexts<out T> : IResult<T> where T : DbContext {}

	public class DbContexts<T> : IContexts<T> where T : DbContext
	{
		readonly IDbContextFactory<T> _factory;

		public DbContexts(IDbContextFactory<T> factory) => _factory = factory;

		public T Get() => _factory.CreateDbContext();
	}

	public interface IQuery<T> : IResult<Expression<Func<DbContext, IQueryable<T>>>> {}

	public class Set<T> : Query<T> where T : class
	{
		public static Set<T> Default { get; } = new Set<T>();

		Set() : base(x => x.Set<T>()) {}
	}

	public class Query<T> : Instance<Expression<Func<DbContext, IQueryable<T>>>>, IQuery<T>
	{
		public Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}
}