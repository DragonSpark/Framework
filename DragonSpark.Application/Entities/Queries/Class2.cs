using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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



	/**/

	public readonly record struct In<T>(DbContext Context, T Parameter);

	public interface IForm<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>> {}

	public readonly struct Invocation<T> : IAsyncDisposable, IDisposable
	{
		readonly IAsyncDisposable _disposable;

		public Invocation(IAsyncDisposable disposable, IAsyncEnumerable<T> elements)
		{
			_disposable = disposable;
			Elements    = elements;
		}

		public IAsyncEnumerable<T> Elements { get; }

		public ValueTask DisposeAsync() => _disposable.DisposeAsync();

		public void Dispose() {}
	}

	public interface IInvoke<in TIn, T> : ISelect<TIn, Invocation<T>> {}

	sealed class Form<T> : Form<None, T>
	{
		public Form(IQuery<None, T> query) : base(query) {}

		public Form(Expression<Func<DbContext, None, IQueryable<T>>> expression) : base(expression) {}
	}

	class Form<TIn, T> : Select<In<TIn>, IAsyncEnumerable<T>>, IForm<TIn, T>
	{
		public Form(IQuery<TIn, T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: base(Compilation.Compile<TIn, T>.Default.Get(expression)) {}
	}

	public class Invoke<TContext, T> : Invoke<TContext, None, T> where TContext : DbContext
	{
		public Invoke(IContexts<TContext> contexts, IQuery<T> query) : base(contexts, new Form<T>(query)) {}

		public Invoke(IContexts<TContext> contexts, IForm<None, T> form) : base(contexts, form) {}
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
	/*public class EvaluateToArray<TContext, TIn, T> : Evaluate<TIn, T, Array<T>>
		where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}*/
}