using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing;

public class Updating<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
{
	protected Updating(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

	protected Updating(IScopes scopes, ISelecting<TIn, TOut> selecting)
		: this(new SelectForEdit<TIn, TOut>(scopes, selecting)) {}

	protected Updating(IEdit<TIn, TOut> select)
		: base(@select, UpdateLocal<TOut>.Default.Then().Operation().Out()) {}
}