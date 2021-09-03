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
		readonly Edit _context;

		public Edit(Edit context, T subject)
		{
			_context = context;
			Subject  = subject;
		}

		public T Subject { get; }

		public void Deconstruct(out DbContext context, out T subject)
		{
			context = _context.Subject;
			subject = Subject;
		}

		public ValueTask DisposeAsync() => _context.DisposeAsync();
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
		}
	}

	public interface IEdit<in TIn, T> : ISelecting<TIn, Edit<T>> {}

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
			var invoke   = _invoke.Get(parameter);
			var evaluate = await _evaluate.Await(invoke.Elements);
			return new(new Edit(invoke.Context), evaluate);
		}
	}
}