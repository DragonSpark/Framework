using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Update<T> : Modify<T> where T : class
{
	protected Update(IScopes scopes) : base(scopes, UpdateLocal<T>.Default.Then().Operation()) {}

	protected Update(IEdit<T, T> edit) : base(edit, UpdateLocal<T>.Default) {}
}

public class Update<TIn, TOut> : Modify<TIn, TOut> where TOut : class
{
	protected Update(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

	protected Update(IScopes scopes, IStopAware<TIn, TOut> selecting)
		: this(new Edits<TIn, TOut>(scopes, selecting)) {}

	protected Update(IEdit<TIn, TOut> select) : base(@select, UpdateLocal<TOut>.Default.Then().Operation().Out()) {}
}