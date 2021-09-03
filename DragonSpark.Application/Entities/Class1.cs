using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class1 {}

	public class Attach<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> save, ISelecting<TIn, T> select,
		                 ICommand<Modification<T>> modification)
			: base(save, select, modification.Execute) {}

		protected Attach(IContexts<TContext> save, ISelecting<TIn, T> select, Action<T> configure)
			: base(save, select, configure) {}

		protected Attach(IContexts<TContext> save, ISelecting<TIn, T> select, Action<Modification<T>> configure)
			: base(save, select, Attach<T>.Default.Then().Append(configure)) {}
	}

	public class Attach<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> save, ICommand<Modification<T>> modification)
			: this(save, modification.Execute) {}

		protected Attach(IContexts<TContext> save, Action<T> configure) : base(save, configure) {}

		protected Attach(IContexts<TContext> save, Action<Modification<T>> configure)
			: base(save, Attach<T>.Default.Then().Append(configure)) {}
	}

	sealed class Attach<T> : Command<Modification<T>> where T : class
	{
		public static Attach<T> Default { get; } = new Attach<T>();

		Attach() : base(x => x.Context.Set<T>().Attach(x.Parameter)) {}
	}

	public class Modify<TContext, T> : IOperation<T> where TContext : DbContext
	{
		readonly IContexts<TContext>     _save;
		readonly Action<Modification<T>> _configure;

		protected Modify(IContexts<TContext> save, ICommand<Modification<T>> modification)
			: this(save, modification.Execute) {}

		protected Modify(IContexts<TContext> save, Action<T> configure) : this(save, x => configure(x.Parameter)) {}

		protected Modify(IContexts<TContext> save, Action<Modification<T>> configure)
		{
			_save      = save;
			_configure = configure;
		}

		public async ValueTask Get(T parameter)
		{
			await using var context = _save.Get();
			_configure(new(context, parameter));
			await context.SaveChangesAsync().ConfigureAwait(false);
		}
	}

	public class Modify<TIn, TContext, T> : IOperation<TIn> where TContext : DbContext
	{
		readonly IContexts<TContext>     _save;
		readonly ISelecting<TIn, T>      _select;
		readonly Action<Modification<T>> _configure;

		protected Modify(IContexts<TContext> save, ISelecting<TIn, T> select, ICommand<Modification<T>> modification)
			: this(save, select, modification.Execute) {}

		protected Modify(IContexts<TContext> save, ISelecting<TIn, T> select, Action<T> configure)
			: this(save, select, x => configure(x.Parameter)) {}

		protected Modify(IContexts<TContext> save, ISelecting<TIn, T> select, Action<Modification<T>> configure)
		{
			_save      = save;
			_select    = select;
			_configure = configure;
		}

		public async ValueTask Get(TIn parameter)
		{
			await using var context   = _save.Get();
			var             entity = await _select.Await(parameter);
			_configure(new(context, entity));
			await context.SaveChangesAsync().ConfigureAwait(false);
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
		protected Remove(IContexts<TContext> contexts)
			: base(contexts, x => x.Context.Set<T>().Remove(x.Parameter)) {}
	}
}