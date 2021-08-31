using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	/*public class Invoke<TContext, T> : Invoke<TContext, None, T> where TContext : DbContext
	{
		public Invoke(IContexts<TContext> contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(contexts, new Form<T>(expression)) {}

		public Invoke(IContexts<TContext> contexts, IForm<None, T> form) : base(contexts, form) {}
	}*/

	public class Invoke<TContext, TIn, T> : IInvoke<TIn, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IForm<TIn, T>       _form;

		public Invoke(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(contexts, new Form<TIn, T>(expression)) {}

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
}