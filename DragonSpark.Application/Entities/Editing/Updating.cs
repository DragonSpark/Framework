using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing;

public class Updating<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
{
	protected Updating(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

	protected Updating(IScopes scopes, ISelecting<TIn, TOut> selecting)
		: this(scopes, selecting, UpdateLocal<TOut>.Default) {}

	protected Updating(IScopes scopes, ISelecting<TIn, TOut> selecting, IModify<TOut> modify)
		: this(new Edits<TIn, TOut>(scopes, selecting), modify) {}

	protected Updating(IEdit<TIn, TOut> select) : this(select, UpdateLocal<TOut>.Default) {}

	protected Updating(IEdit<TIn, TOut> select, IModify<TOut> modify)
		: base(@select, modify.Then().Operation().Out()) {}
}