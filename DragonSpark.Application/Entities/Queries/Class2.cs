using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	public interface IQuery<TIn, T> : IResult<Expression<Func<DbContext, TIn, IQueryable<T>>>> {}

	public class InputQuery<TIn, T, TTo> : Instance<Expression<Func<DbContext, TIn, IQueryable<TTo>>>>, IQuery<TIn, TTo>
		where T : class
	{
		protected InputQuery(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: this((DbContext context, TIn _) => select.Invoke(context.Set<T>())) {}

		protected InputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this((DbContext context, TIn _) => select.Invoke(context, context.Set<T>())) {}

		protected InputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> select)
			: this((context, @in) => select.Invoke(@in, context.Set<T>())) {}

		protected InputQuery(Expression<Func<TIn, DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this((context, @in) => select.Invoke(@in, context, context.Set<T>())) {}

		protected InputQuery(Expression<Func<DbContext, TIn, IQueryable<TTo>>> instance) : base(instance) {}
	}

	/**/

	/**/

	sealed class Form<TIn, T> : IForm<TIn, T>
	{
		readonly Func<DbContext, TIn, IAsyncEnumerable<T>> _select;

		public Form(IQuery<TIn, T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression) :
			this(EF.CompileAsyncQuery(expression)) {}

		public Form(Func<DbContext, TIn, IAsyncEnumerable<T>> select) => _select = select;

		public IAsyncEnumerable<T> Get(In<TIn> parameter) => _select(parameter.Context, parameter.Parameter);
	}

	public class Invoke<TContext, TIn, T> : IInvoke<TIn, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IForm<TIn, T>       _form;

		public Invoke(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, new Form<TIn, T>(query)) {}

		public Invoke(IContexts<TContext> contexts, IForm<TIn, T> form)
		{
			_contexts = contexts;
			_form     = form;
		}

		public Invocation<T> Get(TIn parameter)
		{
			var context = _contexts.Get();
			var form    = _form.Get(new(context, parameter));
			return new(context, form);
		}
	}

	/**/
	public class EvaluateToArray<TContext, TIn, T> : Evaluate<TIn, T, Array<T>>
		where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

}