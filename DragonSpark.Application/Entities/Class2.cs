using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class2 {}

	public readonly struct Edit<T> : IAsyncDisposable
	{
		public static implicit operator In<T>(Edit<T> instance) => new In<T>(instance.Context, instance.Subject);

		public Edit(DbContext context, T subject)
		{
			Context = context;
			Subject = subject;
		}

		public DbContext Context { get; }

		public T Subject { get; }

		public void Deconstruct(out DbContext context, out T subject)
		{
			context = Context;
			subject = Subject;
		}

		public ValueTask DisposeAsync() => Context.DisposeAsync();
	}

	public class EditSingle<TIn, TContext, T> : Edit<TIn, T> where TContext : DbContext
	{
		protected EditSingle(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> query,
		                     Action<T> configure)
			: this(contexts, query.Then().Get(), configure) {}

		protected EditSingle(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<T> configure)
			: base(query.Then().Invoke(contexts).Edit.Single(), configure) {}
	}

	public class Edit<TIn, T> : IOperation<TIn>
	{
		readonly IEdit<TIn, T>   _edit;
		readonly Action<Edit<T>> _configure;

		public Edit(IEdit<TIn, T> edit, ICommand<T> configure) : this(edit, configure.Execute) {}

		public Edit(IEdit<TIn, T> edit, Action<T> configure) : this(edit, x => configure(x.Subject)) {}

		public Edit(IEdit<TIn, T> edit, ICommand<Edit<T>> configure) : this(edit, configure.Execute) {}

		public Edit(IEdit<TIn, T> edit, Action<Edit<T>> configure)
		{
			_edit      = edit;
			_configure = configure;
		}

		public async ValueTask Get(TIn parameter)
		{
			await using var edit = await _edit.Await(parameter);
			_configure(edit);
			await edit.Context.SaveChangesAsync().ConfigureAwait(false);
		}
	}

	public interface IEdit<in TIn, T> : ISelecting<TIn, Edit<T>> {}

	public sealed class StartEdit<TIn, TContext, T> : IEdit<TIn, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly ISelecting<TIn, T>  _select;

		public StartEdit(IContexts<TContext> contexts, ISelecting<TIn, T> select)
		{
			_contexts    = contexts;
			_select = @select;
		}

		public async ValueTask<Edit<T>> Get(TIn parameter)
		{
			var context  = _contexts.Get();
			var instance = await _select.Await(parameter);
			return new(context, instance);
		}
	}

	public class Edit<TIn, T, TResult> : IEdit<TIn, TResult>
	{
		readonly IInvoke<TIn, T>       _invoke;
		readonly IEvaluate<T, TResult> _evaluate;

		public Edit(IInvoke<TIn, T> invoke, IEvaluate<T, TResult> evaluate)
		{
			_invoke   = invoke;
			_evaluate = evaluate;
		}

		public async ValueTask<Edit<TResult>> Get(TIn parameter)
		{
			var (context, elements) = _invoke.Get(parameter);
			var evaluate = await _evaluate.Await(elements);
			return new(context, evaluate);
		}
	}
}