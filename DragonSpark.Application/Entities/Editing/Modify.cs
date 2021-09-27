using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
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

	public class Modify<TContext, T> : Modify<T, TContext, T> where TContext : DbContext
	{
		protected Modify(IContexts<TContext> contexts, IOperation<Edit<T>> modification)
			: this(contexts, modification.Await) {}

		protected Modify(IContexts<TContext> contexts, Await<T> configure)
			: this(contexts, x => configure(x.Subject)) {}

		protected Modify(IContexts<TContext> contexts, Await<Edit<T>> configure)
			: base(contexts.Then().Edit(A.Self<T>().Then().Operation().Out()), configure) {}
	}
}