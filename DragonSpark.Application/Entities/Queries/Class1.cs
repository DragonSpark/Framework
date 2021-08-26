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
		/*protected Set(Expression<Func<IQueryable<T>, IQueryable<T>>> @select)
			: base(context => select.Invoke(context.Set<T>())) {}

		protected Set(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> @select)
			: this(context => select.Invoke(context, context.Set<T>())) {}

		protected Set(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}*/
		public static Set<T> Default { get; } = new Set<T>();

		Set() : base(x => x.Set<T>()) {}
	}

	public class Query<T> : Instance<Expression<Func<DbContext, IQueryable<T>>>>, IQuery<T>
	{
		public Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}

	/*public class Query<T, TTo> : Query<TTo>, IQuery<TTo>
	{
		public Query(Expression<Func<DbContext, IQueryable<TTo>>> previous,
		             Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: this(context => select.Invoke(context.Set<T>())) {}

		public Query(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this(context => select.Invoke(context, context.Set<T>())) {}

		public Query(Expression<Func<DbContext, IQueryable<TTo>>> instance) : base(instance) {}
	}*/



	/*public class Set<T, TTo> : Query<TTo>, IQuery<TTo> where T : class
	{
		public Set(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: this(context => select.Invoke(context.Set<T>())) {}

		public Set(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this(context => select.Invoke(context, context.Set<T>())) {}

		public Set(Expression<Func<DbContext, IQueryable<TTo>>> instance) : base(instance) {}
	}*/
}