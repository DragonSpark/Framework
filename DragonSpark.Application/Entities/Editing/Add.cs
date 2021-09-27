using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Editing
{
	public class Add<TIn, TContext, TOut> : ConfiguringResult<TIn, TOut> where TContext : DbContext where TOut : class
	{
		protected Add(ISelecting<TIn, TOut> @new, Save<TContext, TOut> add) : base(@new, add) {}

		protected Add(ISelecting<TIn, TOut> select, IOperation<TOut> operation) : base(select, operation) {}

		protected Add(Await<TIn, TOut> @new, Await<TOut> add) : base(@new, add) {}
	}

	public class Add<TIn, TOut> : Modify<TIn, TOut> where TOut : class
	{
		protected Add(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

		protected Add(IScopes scopes, ISelecting<TIn, TOut> selecting)
			: this(new SelectForEdit<TIn, TOut>(scopes, selecting)) {}

		protected Add(IEdit<TIn, TOut> select) : base(@select, Add<TOut>.Default.Then().Operation().Out()) {}

	}

	public class Adding<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
	{
		protected Adding(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

		protected Adding(IScopes scopes, ISelecting<TIn, TOut> selecting)
			: this(new SelectForEdit<TIn, TOut>(scopes, selecting)) {}

		protected Adding(IEdit<TIn, TOut> select) : base(@select, Add<TOut>.Default.Then().Operation().Out()) {}

	}
}