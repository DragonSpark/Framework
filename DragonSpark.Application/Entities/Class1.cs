using DragonSpark.Application.Entities.Queries.Composition;
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
		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, ICommand<In<T>> modification)
			: this(contexts, query, modification.Execute) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<T> configure)
			: this(contexts, query, x => configure(x.Parameter)) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<In<T>> configure)
			: base(contexts, query, Attach<T>.Default.Then().Append(configure)) {}

		protected Attach(IEdit<TIn, T> @select, ICommand<T> configure) : this(@select, configure.Execute) {}

		protected Attach(IEdit<TIn, T> @select, Action<T> configure) : this(@select, x => configure(x.Parameter)) {}

		protected Attach(IEdit<TIn, T> @select, ICommand<In<T>> configure) : this(@select, configure.Execute) {}

		protected Attach(IEdit<TIn, T> @select, Action<In<T>> configure)
			: base(@select, Attach<T>.Default.Then().Append(configure)) {}
	}

	public class Attach<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> save, ICommand<In<T>> modification) : this(save, modification.Execute) {}

		protected Attach(IContexts<TContext> save, Action<T> configure) : this(save, x => configure(x.Parameter)) {}

		protected Attach(IContexts<TContext> save, Action<In<T>> configure)
			: base(save, Attach<T>.Default.Then().Append(configure)) {}
	}

	sealed class Attach<T> : Command<In<T>>, IModify<T> where T : class
	{
		public static Attach<T> Default { get; } = new Attach<T>();

		Attach() : base(x => x.Context.Set<T>().Attach(x.Parameter)) {}
	}

	public class Modify<TContext, T> : Modify<T, TContext, T> where TContext : DbContext
	{
		protected Modify(IContexts<TContext> save, ICommand<In<T>> modification) : this(save, modification.Execute) {}

		protected Modify(IContexts<TContext> save, Action<T> configure) : this(save, x => configure(x.Parameter)) {}

		protected Modify(IContexts<TContext> save, Action<In<T>> configure)
			: base(save.Then().Edit(A.Self<T>().Then().Operation().Out()), configure) {}
	}

	public class Modify<TIn, TContext, T> : IOperation<TIn> where TContext : DbContext
	{
		readonly IEdit<TIn, T> _select;
		readonly Action<In<T>> _configure;

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, ICommand<In<T>> modification)
			: this(query.Then().Invoke(contexts).Edit.Single(), modification) {}

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<T> configure)
			: this(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<In<T>> configure)
			: this(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected Modify(IEdit<TIn, T> select, ICommand<T> configure) : this(@select, configure.Execute) {}

		protected Modify(IEdit<TIn, T> select, Action<T> configure) : this(@select, x => configure(x.Parameter)) {}

		protected Modify(IEdit<TIn, T> select, ICommand<In<T>> configure) : this(select, configure.Execute) {}

		protected Modify(IEdit<TIn, T> select, Action<In<T>> configure)
		{
			_select    = select;
			_configure = configure;
		}

		public async ValueTask Get(TIn parameter)
		{
			await using var edit = await _select.Get(parameter);
			_configure(edit);
			await edit.Context.SaveChangesAsync().ConfigureAwait(false);
		}
	}

	public interface IModify<T> : ICommand<In<T>> {}

	public class Remove<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, ICommand<T> configure)
			: this(contexts, query, configure.Execute) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<T> configure)
			: base(contexts, query, x => configure(x.Parameter)) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query, (In<T> _) => {}) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, ICommand<In<T>> configure)
			: this(contexts, query, configure.Execute) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Action<In<T>> configure)
			: base(contexts, query, Start.A.Command(configure).Append(Remove<T>.Default)) {}

		protected Remove(IEdit<TIn, T> @select, ICommand<T> configure) : this(@select, configure.Execute) {}

		protected Remove(IEdit<TIn, T> @select, Action<T> configure) : this(@select, x => configure(x.Parameter)) {}

		protected Remove(IEdit<TIn, T> @select, ICommand<In<T>> configure) : this(@select, configure.Execute) {}

		protected Remove(IEdit<TIn, T> @select, Action<In<T>> configure)
			: base(@select, Start.A.Command(configure).Append(Remove<T>.Default)) {}
	}

	/*
	public class Single<TIn, TContext, T> : Operation<TIn> where TContext : DbContext where T : class
	{
		protected Single(IContexts<TContext> contexts, IQuery<TIn, T> query, IModify<TIn> modify)
			: this(contexts.Then().Use(query).To.Single(), modify) {}

		protected Single(ISelecting<TIn, T> entity, IOperation<In<T>> remove)
			: base(entity.Then().Terminate(remove)) {}

	}
	*/

	public class Remove<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts) : base(contexts, Remove<T>.Default) {}
	}

	public sealed class Remove<T> : Command<In<T>>, IModify<T> where T : class
	{
		public static Remove<T> Default { get; } = new Remove<T>();

		Remove() : base(x => x.Context.Set<T>().Remove(x.Parameter)) {}
	}
}