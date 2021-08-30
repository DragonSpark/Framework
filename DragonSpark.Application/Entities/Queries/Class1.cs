using DragonSpark.Model;
using DragonSpark.Model.Results;
using LinqKit;
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

	public interface IQuery<T> : IQuery<None, T> {}

	public class Query<T> : InputQuery<None, T>, IQuery<T>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<T>>>(Query<T> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		protected Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}





	/*public class Introduce<TFrom, T1, T2, T3, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                 Expression<Func<DbContext, IQueryable<T3>>>) others,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                 IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context), others.Item3.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                 Expression<Func<DbContext, IQueryable<T3>>>) others,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                 IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context), others.Item3.Invoke(context))) {}
	}



	public class StartIntroduce<TFrom, T1, T2, T3, TTo> : Introduce<TFrom, T1, T2, T3, TTo> where TFrom : class
	{
		public StartIntroduce((Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                      Expression<Func<DbContext, IQueryable<T3>>>) others,
		                      Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                      IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}

		public StartIntroduce((Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                      Expression<Func<DbContext, IQueryable<T3>>>) others,
		                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                      IQueryable<T3>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}
	}*/

	/**/

	/**/

	/*public class EvaluateToArray<TContext, T> : Evaluate<T, Array<T>> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

	public class EvaluateToLease<TContext, T> : Evaluate<T, Lease<T>> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}

	public class EvaluateToList<TContext, T> : Evaluate<T, List<T>> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke, ToList<T>.Default) {}
	}

	public class EvaluateToDictionary<TContext, T, TKey> : Evaluate<T, Dictionary<TKey, T>> where TKey : notnull
	                                                                                        where TContext : DbContext
	{
		public EvaluateToDictionary(IContexts<TContext> contexts, IQuery<T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToDictionary(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToDictionary<TContext, T, TKey, TValue> : Evaluate<T, Dictionary<TKey, TValue>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToDictionary(IContexts<TContext> contexts, IQuery<T> query,
		                            IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, T>(contexts, query), evaluate) {}

		public EvaluateToDictionary(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToSingle<TContext, T> : Evaluate<T, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke, Single<T>.Default) {}
	}

	public class EvaluateToSingleOrDefault<TContext, T> : Evaluate<T, T?> where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<None, T> invoke) : base(invoke, SingleOrDefault<T>.Default) {}
	}

	public class EvaluateToFirst<TContext, T> : Evaluate<T, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke, First<T>.Default) {}
	}

	public class EvaluateToFirstOrDefault<TContext, T> : Evaluate<T, T?> where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}

	public class EvaluateToAny<TContext, T> : Evaluate<T, bool> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke, Any<T>.Default) {}
	}*/
}