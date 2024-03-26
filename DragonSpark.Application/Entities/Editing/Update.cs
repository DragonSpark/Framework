using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Editing;

public class Update<T> : Modify<T> where T : class
{
	protected Update(IContexts contexts) : base(contexts, UpdateLocal<T>.Default.Then().Operation()) {}

	protected Update(IEdit<T, T> edit) : base(edit, UpdateLocal<T>.Default.Then().Operation()) {}
}

public class Update<TIn, TOut> : Modify<TIn, TOut> where TOut : class
{
	protected Update(IContexts contexts, IQuery<TIn, TOut> query) : this(contexts.Then().Use(query).Edit.Single()) {}

	protected Update(IContexts contexts, ISelecting<TIn, TOut> selecting)
		: this(new Edits<TIn, TOut>(contexts, selecting)) {}

	protected Update(IEdit<TIn, TOut> select) : base(@select, UpdateLocal<TOut>.Default.Then().Operation().Out()) {}
}