using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class1 {}

	public readonly struct SaveOperation<T> : IAsyncDisposable where T : DbContext
	{
		public SaveOperation(T subject) => Subject = subject;

		public T Subject { get; }

		public async ValueTask DisposeAsync()
		{
			await Subject.SaveChangesAsync().ConfigureAwait(false);
			await Subject.DisposeAsync().ConfigureAwait(false);
		}
	}

	public interface ISaveContext<T> : IResult<SaveOperation<T>> where T : DbContext {}

	public class SaveContext<T> : ISaveContext<T> where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public SaveContext(IContexts<T> contexts) => _contexts = contexts;

		public SaveOperation<T> Get() => new(_contexts.Get());
	}

	public class Attach<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(ISaveContext<TContext> save, ISelecting<TIn, T> @select,
		                 ICommand<Modification<T>> modification)
			: base(save, @select, modification.Execute) {}

		protected Attach(ISaveContext<TContext> save, ISelecting<TIn, T> @select, Action<T> configure)
			: base(save, @select, configure) {}

		protected Attach(ISaveContext<TContext> save, ISelecting<TIn, T> @select, Action<Modification<T>> configure)
			: base(save, @select, Attach<T>.Default.Then().Append(configure)) {}
	}

	public class Attach<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(ISaveContext<TContext> save, ICommand<Modification<T>> modification)
			: this(save, modification.Execute) {}

		protected Attach(ISaveContext<TContext> save, Action<T> configure) : base(save, configure) {}

		protected Attach(ISaveContext<TContext> save, Action<Modification<T>> configure)
			: base(save, Attach<T>.Default.Then().Append(configure)) {}
	}

	sealed class Attach<T> : Command<Modification<T>> where T : class
	{
		public static Attach<T> Default { get; } = new Attach<T>();

		Attach() : base(x => x.Context.Set<T>().Attach(x.Parameter)) {}
	}

	public class Modify<TContext, T> : IOperation<T> where TContext : DbContext
	{
		readonly ISaveContext<TContext>  _save;
		readonly Action<Modification<T>> _configure;

		protected Modify(ISaveContext<TContext> save, ICommand<Modification<T>> modification)
			: this(save, modification.Execute) {}

		protected Modify(ISaveContext<TContext> save, Action<T> configure) : this(save, x => configure(x.Parameter)) {}

		protected Modify(ISaveContext<TContext> save, Action<Modification<T>> configure)
		{
			_save      = save;
			_configure = configure;
		}

		public async ValueTask Get(T parameter)
		{
			await using var save = _save.Get();
			_configure(new(save.Subject, parameter));
		}
	}

	public class Modify<TIn, TContext, T> : IOperation<TIn> where TContext : DbContext
	{
		readonly ISaveContext<TContext>  _save;
		readonly ISelecting<TIn, T>      _select;
		readonly Action<Modification<T>> _configure;

		protected Modify(ISaveContext<TContext> save, ISelecting<TIn, T> select, ICommand<Modification<T>> modification)
			: this(save, select, modification.Execute) {}

		protected Modify(ISaveContext<TContext> save, ISelecting<TIn, T> select, Action<T> configure)
			: this(save, @select, x => configure(x.Parameter)) {}

		protected Modify(ISaveContext<TContext> save, ISelecting<TIn, T> select, Action<Modification<T>> configure)
		{
			_save      = save;
			_select    = @select;
			_configure = configure;
		}

		public async ValueTask Get(TIn parameter)
		{
			await using var save   = _save.Get();
			var             entity = await _select.Await(parameter);
			_configure(new(save.Subject, entity));
		}
	}

	public interface IModify<T> : ICommand<Modification<T>> {}

	public readonly record struct Modification<T>(DbContext Context, T Parameter);

	public class Remove<TIn, TContext, T> : Operation<TIn> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Remove<TContext, T> remove)
			: this(contexts.Then().Use(query).To.Single(), remove) {}

		protected Remove(ISelecting<TIn, T> entity, IOperation<T> remove)
			: base(entity.Then().Terminate(remove)) {}
	}

	public class Remove<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(ISaveContext<TContext> contexts)
			: base(contexts, x => x.Context.Set<T>().Remove(x.Parameter)) {}
	}
}