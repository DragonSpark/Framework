using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

	public class Query<T> : DragonSpark.Model.Results.Instance<Expression<Func<DbContext, IQueryable<T>>>>, IQuery<T>
	{
		public Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}

	public class InputQuery<TIn, T> : InputQuery<TIn, T, T> where T : class
	{
		protected InputQuery(Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<TIn, DbContext, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> select) : base(select) {}
	}

	public class Combine<T> : Combine<T, T>
	{
		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(previous, select) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(previous, select) {}
	}

	public class Start<T> : Combine<T> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class Start<T, TTo> : Combine<T, TTo> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class Combine<T, TTo> : Query<TTo>
	{
		public Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		               Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(previous.Invoke(context))) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, previous.Invoke(context))) {}
	}

	public class StartWhere<T> : Where<T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<T>.Default, where) {}
	}

	public class Where<T> : Combine<T>
	{
		public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: base(previous, x => x.Where(where)) {}
	}

	public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<T>.Default, where, select) {}
	}

	public class WhereSelect<T, TTo> : Combine<T, TTo>
	{
		protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<T, TTo>> select)
			: base(previous, x => x.Where(where).Select(select)) {}
	}

	public class StartWhereMany<T, TTo> : WhereMany<T, TTo> where T : class
	{
		public StartWhereMany(Expression<Func<T, bool>> @where, Expression<Func<T, IEnumerable<TTo>>> @select)
			: base(Set<T>.Default, @where, @select) {}
	}

	public class WhereMany<T, TTo> : Combine<T, TTo>
	{
		public WhereMany(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                 Expression<Func<T, IEnumerable<TTo>>> select)
			: base(previous, x => x.Where(where).SelectMany(select)) {}
	}

	public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default, select) {}
	}

	public class Select<TFrom, TTo> : Combine<TFrom, TTo>
	{
		public Select(Expression<Func<DbContext, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: base(previous, x => x.Select(select)) {}
	}

	public class StartSelectMany<TFrom, TTo> : SelectMany<TFrom, TTo> where TFrom : class
	{
		protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(Set<TFrom>.Default, select) {}
	}

	public class SelectMany<TFrom, TTo> : Combine<TFrom, TTo>
	{
		public SelectMany(Expression<Func<DbContext, IQueryable<TFrom>>> previous,
		                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(previous, x => x.SelectMany(select)) {}
	}

	public class StartIntroduce<TFrom, TOther, TTo> : Introduce<TFrom, TOther, TTo> where TFrom : class
	{
		public StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                      Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, other, select) {}

		public StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                      select)
			: base(Set<TFrom>.Default, other, select) {}
	}

	public class StartIntroduce<TFrom, T1, T2, TTo> : Introduce<TFrom, T1, T2, TTo> where TFrom : class
	{
		public StartIntroduce(
			(Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>) others,
			Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}

		public StartIntroduce(
			(Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>) others,
			Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}
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
	}

	public class Introduce<TFrom, TOther, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, from.Invoke(context), other.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), other.Invoke(context))) {}
	}

	public class Introduce<TFrom, T1, T2, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>)
			                 others,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>>
			                 select)
			: base(context => select.Invoke(context, from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>)
			                 others,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context))) {}
	}

	public class Introduce<TFrom, T1, T2, T3, TTo> : Query<TTo>
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

	/**/

	sealed class Form<T> : IForm<None, T>
	{
		readonly Func<DbContext, IAsyncEnumerable<T>> _select;

		public Form(IQuery<T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, IQueryable<T>>> expression) : this(EF.CompileAsyncQuery(expression)) {}

		public Form(Func<DbContext, IAsyncEnumerable<T>> select) => _select = select;

		public IAsyncEnumerable<T> Get(In<None> parameter) => _select(parameter.Context);
	}

	public class Invoke<TContext, T> : Invoke<TContext, None, T> where TContext : DbContext
	{
		public Invoke(IContexts<TContext> contexts, IQuery<T> query) : base(contexts, new Form<T>(query)) {}

		public Invoke(IContexts<TContext> contexts, IForm<None, T> form) : base(contexts, form) {}
	}

	/**/

	public class EvaluateToArray<TContext, T> : Evaluate<T, Array<T>> where TContext : DbContext
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
	}

	public class Evaluate<T, TResult> : IResulting<TResult>
	{
		readonly IInvoke<None, T>      _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		public Evaluate(IInvoke<None, T> invoke, IEvaluate<T, TResult> evaluate)
		{
			_invoke   = invoke;
			_evaluate = evaluate;
		}

		public async ValueTask<TResult> Get()
		{
			await using var invocation = _invoke.Get();
			var             result     = await _evaluate.Get(invocation.Elements);
			return result;
		}
	}
}