using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public readonly struct Edit<T> : IEditor
	{
		readonly IEditor _context;

		public Edit(IEditor context, T subject)
		{
			Subject  = subject;
			_context = context;
		}

		public T Subject { get; }

		public ValueTask Get() => _context.Get();

		public void Add(object entity)
		{
			_context.Add(entity);
		}

		public void Attach(object entity)
		{
			_context.Attach(entity);
		}

		public void Update(object entity)
		{
			_context.Update(entity);
		}

		public void Remove(object entity)
		{
			_context.Remove(entity);
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		public void Deconstruct(out IEditor context, out T subject)
		{
			context = _context;
			subject = Subject;
		}
	}

	public readonly struct ManyEdit<T> : IEditor
	{
		readonly IEditor _context;

		public static implicit operator Memory<T>(ManyEdit<T> instance) => instance.Subject;

		public ManyEdit(IEditor context, Leasing<T> subject)
		{
			Subject  = subject;
			_context = context;
		}

		public Leasing<T> Subject { get; }

		public void Dispose()
		{
			_context.Dispose();
			Subject.Dispose();
		}

		public ValueTask Get() => _context.Get();

		public void Add(object entity)
		{
			_context.Add(entity);
		}

		public void Attach(object entity)
		{
			_context.Attach(entity);
		}

		public void Update(object entity)
		{
			_context.Update(entity);
		}

		public void Remove(object entity)
		{
			_context.Remove(entity);
		}
	}

	public interface IEdit<in TIn, T> : ISelecting<TIn, Edit<T>> {}

	public sealed class SelectForEdit<TIn, T> : IEdit<TIn, T>
	{
		readonly IInvocations       _invocations;
		readonly ISelecting<TIn, T> _select;

		public SelectForEdit(IInvocations invocations, ISelecting<TIn, T> select)
		{
			_invocations = invocations;
			_select      = @select;
		}

		public async ValueTask<Edit<T>> Get(TIn parameter)
		{
			var (context, disposable) = _invocations.Get();
			var instance = await _select.Await(parameter);
			return new(new Editor(context, await disposable.Await()), instance);
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
			var (context, disposable, elements) = await _invoke.Await(parameter);
			var evaluate = await _evaluate.Await(elements);
			return new(new Editor(context, disposable), evaluate);
		}
	}

	public class EditExisting<T, TContext> : EditExisting<T> where TContext : DbContext where T : class
	{
		protected EditExisting(IContexts<TContext> contexts)
			: base(new AmbientAwareInvocations(contexts.Then().Invocations())) {}
	}

	public class EditExisting<T> : ISelecting<T, Edit<T>> where T : class
	{
		readonly IInvocations _invocations;

		protected EditExisting(IInvocations invocations) => _invocations = invocations;

		public async ValueTask<Edit<T>> Get(T parameter)
		{
			var (context, disposable) = _invocations.Get();
			var editor = new Editor(context, await disposable.Await());
			editor.Attach(parameter);
			await context.Entry(parameter).ReloadAsync().ConfigureAwait(false);
			return new(editor, parameter);
		}
	}

	public class Edit<TIn, T> : Selecting<TIn, Edit<T>>, IEdit<TIn, T>
	{
		protected Edit(ISelect<TIn, ValueTask<Edit<T>>> @select) : base(@select) {}

		protected Edit(Func<TIn, ValueTask<Edit<T>>> @select) : base(@select) {}
	}

	public class Editing<TIn, TContext, T> : Edit<TIn, T> where TContext : DbContext
	{
		protected Editing(IContexts<TContext> context, IQuery<TIn, T> query)
			: base(context.Then().Use(query).Edit.Single()) {}
	}

	public interface IEditMany<in TIn, T> : ISelecting<TIn, ManyEdit<T>> {}

	public class EditMany<TIn, TContext, T> : IEditMany<TIn, T> where TContext : DbContext
	{
		readonly IEdit<TIn, Leasing<T>> _edit;

		protected EditMany(IContexts<TContext> context, IQuery<TIn, T> query)
			: this(context.Then().Use(query).Edit.Lease()) {}

		protected EditMany(IEdit<TIn, Leasing<T>> edit) => _edit = edit;

		public async ValueTask<ManyEdit<T>> Get(TIn parameter)
		{
			var (editor, subject) = await _edit.Await(parameter);
			return new(editor, subject);
		}
	}

	public class EditCombined<TIn, TContext, T> : IEditMany<TIn, T> where TContext : DbContext
	{
		readonly IEdit<TIn, Leasing<T>> _first, _second;

		protected EditCombined(IContexts<TContext> context, IQuery<TIn, T> first, IQuery<TIn, T> second)
			: this(context.Then().Use(first).Edit.Lease(), context.Then().Use(second).Edit.Lease()) {}

		protected EditCombined(IEdit<TIn, Leasing<T>> first, IEdit<TIn, Leasing<T>> second)
		{
			_first  = first;
			_second = second;
		}

		public async ValueTask<ManyEdit<T>> Get(TIn parameter)
		{
			var (editor, first) = await _first.Await(parameter);
			var (_, second)     = await _second.Await(parameter);
			var combined = first.Then().Concat(second).Result();
			second.Dispose();
			return new(editor, combined);
		}
	}

	public interface IEditor : IOperation, IDisposable
	{
		void Add(object entity);

		void Attach(object entity);

		void Update(object entity);

		void Remove(object entity);
	}

	sealed class Editor : IEditor
	{
		readonly DbContext   _context;
		readonly IDisposable _disposable;

		public Editor(DbContext context) : this(context, context) {}

		public Editor(DbContext context, IDisposable disposable)
		{
			_context    = context;
			_disposable = disposable;
		}

		public async ValueTask Get()
		{
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public void Add(object entity)
		{
			_context.Add(entity);
		}

		public void Attach(object entity)
		{
			_context.Attach(entity);
		}

		public void Update(object entity)
		{
			_context.Update(entity);
		}

		public void Remove(object entity)
		{
			_context.Remove(entity);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}