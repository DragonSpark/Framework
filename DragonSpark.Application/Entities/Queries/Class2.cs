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

	/*public class StartInputQuery<TIn, T> : InputQuery<TIn, T>
	{
		protected StartInputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> instance) : base(instance) {}
	}*/

	public class InputQuery<TIn, T> : Instance<Expression<Func<DbContext, TIn, IQueryable<T>>>>, IQuery<TIn, T>
	{
		protected InputQuery(Expression<Func<DbContext, IQueryable<T>>> instance)
			: base((context, _) => instance.Invoke(context)) {}

		protected InputQuery(Expression<Func<TIn, IQueryable<T>>> instance)
			: base((context, @in) => instance.Invoke(@in)) {}

		protected InputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> instance) : base(instance) {}
	}

	public class InputCombine<TIn, T, TTo> : InputQuery<TIn, TTo>
	{
		protected InputCombine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                       Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, previous.Invoke(context, @in))) {}

		protected InputCombine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                       Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(previous.Invoke(context, @in))) {}

		protected InputCombine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                       Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(@in, previous.Invoke(context, @in))) {}

		protected InputCombine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                       Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, @in, previous.Invoke(context, @in))) {}
	}

	public class Set<TIn, T> : InputQuery<TIn, T> where T : class
	{
		public static Set<TIn, T> Default { get; } = new();

		Set() : base(x => x.Set<T>()) {}
	}

	public class StartInputQuery<TIn, T> : StartInputQuery<TIn, T, T> where T : class
	{
		protected StartInputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> instance)
			: base(instance) {}

		protected StartInputQuery(Expression<Func<IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

		protected StartInputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

		protected StartInputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                          Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T>>> instance)
			: base(previous, instance) {}
	}

	public class StartInputQuery<TIn, T, TTo> : InputCombine<TIn, T, TTo> where T : class
	{
		protected StartInputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInputQuery(Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                     Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(previous, instance) {}
	}

	/**/

	/**/

	sealed class Form<TIn, T> : IForm<TIn, T>
	{
		readonly IWrapper<TIn, T> _select;

		public Form(IQuery<TIn, T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(Wrappers<TIn, T>.Default.Get(expression)) {}

		public Form(IWrapper<TIn, T> select) => _select = select;

		public IAsyncEnumerable<T> Get(In<TIn> parameter) => _select.Get(parameter);
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