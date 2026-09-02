using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Adding<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
{
	protected Adding(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes, query, AddLocal<TOut>.Default) {}

	protected Adding(IScopes scopes, IQuery<TIn, TOut> query, IModify<TOut> modify)
		: this(scopes.Then().Use(query).Edit.Single(), modify) {}

	protected Adding(IScopes scopes, IStopAware<TIn, TOut> selecting)
		: this(scopes, selecting, AddLocal<TOut>.Default) {}

	protected Adding(IScopes scopes, IStopAware<TIn, TOut> selecting, IModify<TOut> modify)
		: this(new Edits<TIn, TOut>(scopes, selecting), modify) {}

	protected Adding(IEdit<TIn, TOut> select) : this(select, AddLocal<TOut>.Default) {}

	protected Adding(IEdit<TIn, TOut> select, IModify<TOut> modify) : base(select, modify.Then().Operation()) {}
}