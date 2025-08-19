using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Updating<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
{
	protected Updating(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

	/*protected Updating(IScopes scopes, ISelecting<TIn, TOut> selecting)
		: this(scopes, selecting.AsStop()) {}*/

	protected Updating(IScopes scopes, IStopAware<TIn, TOut> selecting)
		: this(scopes, selecting, UpdateLocal<TOut>.Default) {}

	protected Updating(IScopes scopes, IStopAware<TIn, TOut> selecting, IModify<TOut> modify)
		: this(new Edits<TIn, TOut>(scopes, selecting), modify) {}

	protected Updating(IEdit<TIn, TOut> select) : this(select, UpdateLocal<TOut>.Default) {}

	protected Updating(IEdit<TIn, TOut> select, IModify<TOut> modify) : base(select, modify.Then().Operation()) {}
}