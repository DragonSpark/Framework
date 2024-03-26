using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Editing;

public class Updating<TIn, TOut> : Modifying<TIn, TOut> where TOut : class
{
	protected Updating(IContexts contexts, IQuery<TIn, TOut> query) : this(contexts.Then().Use(query).Edit.Single()) {}

	protected Updating(IContexts contexts, ISelecting<TIn, TOut> selecting)
		: this(contexts, selecting, UpdateLocal<TOut>.Default) {}

	protected Updating(IContexts contexts, ISelecting<TIn, TOut> selecting, IModify<TOut> modify)
		: this(new Edits<TIn, TOut>(contexts, selecting), modify) {}

	protected Updating(IEdit<TIn, TOut> select) : this(select, UpdateLocal<TOut>.Default) {}

	protected Updating(IEdit<TIn, TOut> select, IModify<TOut> modify)
		: base(@select, modify.Then().Operation().Out()) {}
}