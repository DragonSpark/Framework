using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public class ContextualModify<TIn, TContext, T> : Modify<TIn, T> where TContext : DbContext
	{
		protected ContextualModify(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
			: base(query.Then().Invoke(contexts).Edit.Single(), modification) {}

		protected ContextualModify(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: base(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected ContextualModify(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: base(query.Then().Invoke(contexts).Edit.Single(), configure) {}

		protected ContextualModify(IEdit<TIn, T> @select, IOperation<T> configure) : base(@select, configure) {}

		protected ContextualModify(IEdit<TIn, T> @select, Await<T> configure) : base(@select, configure) {}

		protected ContextualModify(IEdit<TIn, T> @select, IOperation<Edit<T>> configure) : base(@select, configure) {}

		protected ContextualModify(IEdit<TIn, T> @select, Await<Edit<T>> configure) : base(@select, configure) {}
	}

	public class ContextualModify<TContext, T> : ContextualModify<T, TContext, T> where TContext : DbContext
	{
		protected ContextualModify(IContexts<TContext> contexts, IOperation<Edit<T>> modification)
			: this(contexts, modification.Await) {}

		protected ContextualModify(IContexts<TContext> contexts, Await<T> configure)
			: this(contexts, x => configure(x.Subject)) {}

		protected ContextualModify(IContexts<TContext> contexts, Await<Edit<T>> configure)
			: base(contexts.Then().Edit(A.Self<T>().Then().Operation().Out()), configure) {}
	}

	public class Modify<TIn, T> : IOperation<TIn>
	{
		readonly IEdit<TIn, T>  _select;
		readonly Await<Edit<T>> _configure;

		protected Modify(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Await) {}

		protected Modify(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

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

	// TODO: organize

	public class Modifying<TIn, T> : ISelecting<TIn, T>
	{
		readonly IEdit<TIn, T>  _select;
		readonly Await<Edit<T>> _configure;

		protected Modifying(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Await) {}

		protected Modifying(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

		protected Modifying(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Await) {}

		protected Modifying(IEdit<TIn, T> select, Await<Edit<T>> configure)
		{
			_select    = select;
			_configure = configure;
		}

		public async ValueTask<T> Get(TIn parameter)
		{
			using var edit = await _select.Get(parameter);
			await _configure(edit);
			await edit.Await();
			return edit.Subject;
		}
	}

}