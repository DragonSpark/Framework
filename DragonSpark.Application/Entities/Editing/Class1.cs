using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	class Class1 {}

	public class Attach<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
			: this(contexts, query, modification.Await) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: this(contexts, query, x => configure(x.Subject)) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: base(contexts, query, Attach<T>.Default.Then().Operation().Append(configure)) {}

		protected Attach(IEdit<TIn, T> @select, IOperation<T> configure) : this(@select, configure.Await) {}

		protected Attach(IEdit<TIn, T> @select, Await<T> configure) : this(@select, x => configure(x.Subject)) {}

		protected Attach(IEdit<TIn, T> @select, IOperation<Edit<T>> configure) : this(@select, configure.Await) {}

		protected Attach(IEdit<TIn, T> @select, Await<Edit<T>> configure)
			: base(@select, Attach<T>.Default.Then().Operation().Append(configure)) {}
	}

	public class Attach<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> contexts, IOperation<Edit<T>> modification)
			: this(contexts, modification.Await) {}

		protected Attach(IContexts<TContext> contexts, Await<T> configure)
			: this(contexts, x => configure(x.Subject)) {}

		protected Attach(IContexts<TContext> contexts, Await<Edit<T>> configure)
			: base(contexts, Attach<T>.Default.Then().Operation().Append(configure)) {}
	}

	public sealed class Attach<T> : Command<Edit<T>>, IModify<T> where T : class
	{
		public static Attach<T> Default { get; } = new Attach<T>();

		Attach() : base(x => x.Attach(x.Subject)) {}
	}

	public class Modify<TContext, T> : Modify<T, TContext, T> where TContext : DbContext
	{
		protected Modify(IContexts<TContext> contexts, IOperation<Edit<T>> modification)
			: this(contexts, modification.Await) {}

		protected Modify(IContexts<TContext> contexts, Await<T> configure)
			: this(contexts, x => configure(x.Subject)) {}

		protected Modify(IContexts<TContext> contexts, Await<Edit<T>> configure)
			: base(contexts.Then().Edit(A.Self<T>().Then().Operation().Out()), configure) {}
	}

	public class Modify<TIn, TContext, T> : IOperation<TIn> where TContext : DbContext
	{
		readonly IEdit<TIn, T>  _select;
		readonly Await<Edit<T>> _configure;

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
			: this(query.Then().Invoke(contexts).Edit.Single(), modification) {}

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: this(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected Modify(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: this(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected Modify(IEdit<TIn, T> select, IOperation<T> configure) : this(@select, configure.Await) {}

		protected Modify(IEdit<TIn, T> select, Await<T> configure) : this(@select, x => configure(x.Subject)) {}

		protected Modify(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Await) {}

		protected Modify(IEdit<TIn, T> select, Await<Edit<T>> configure)
		{
			_select    = select;
			_configure = configure;
		}

		public async ValueTask Get(TIn parameter)
		{
			using var edit = await _select.Get(parameter);
			await _configure(edit);
			await edit.Await();
		}
	}

	public class SelectedAttach<TContext, TFrom, TTo> : Modify<TContext, TFrom>
		where TContext : DbContext where TTo : class
	{
		protected SelectedAttach(IContexts<TContext> contexts, IOperation<Edit<TFrom>> from, Func<TFrom, TTo> select)
			: base(contexts,
			       Start.A.Selection<Edit<TFrom>>()
			            .By.Calling(x =>
			                        {
				                        var (editor, subject) = x;
				                        return new Edit<TTo>(editor, select(subject));
			                        })
			            .Terminate(Attach<TTo>.Default)
			            .Operation()
			            .Append(from)) {}
	}

	public interface IModify<T> : ICommand<Edit<T>> {}

	public interface IModifying<T> : IOperation<Edit<T>> {}

	public class Remove<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<T> configure)
			: this(contexts, query, configure.Await) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: base(contexts, query, x => configure(x.Subject)) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query, (Edit<T> _) => Task.CompletedTask.ToOperation().ConfigureAwait(false)) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> configure)
			: this(contexts, query, configure.Await) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: base(contexts, query, Start.An.Operation(configure).Append(Remove<T>.Default)) {}

		protected Remove(IEdit<TIn, T> @select, IOperation<T> configure) : this(@select, configure.Await) {}

		protected Remove(IEdit<TIn, T> @select, Await<T> configure) : this(@select, x => configure(x.Subject)) {}

		protected Remove(IEdit<TIn, T> @select, IOperation<Edit<T>> configure) : this(@select, configure.Await) {}

		protected Remove(IEdit<TIn, T> @select, Await<Edit<T>> configure)
			: base(@select, Start.An.Operation(configure).Append(Remove<T>.Default)) {}
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
		protected Remove(IContexts<TContext> contexts) : base(contexts, Remove<T>.Default.Then().Operation()) {}
	}

	public sealed class Remove<T> : Command<Edit<T>>, IModify<T> where T : class
	{
		public static Remove<T> Default { get; } = new Remove<T>();

		Remove() : base(x => x.Remove(x.Subject)) {}
	}
}