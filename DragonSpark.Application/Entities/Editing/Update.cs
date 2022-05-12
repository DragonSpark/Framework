using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing;

public class Update<T> : Modify<T> where T : class
{
	protected Update(IScopes scopes) : base(scopes, UpdateLocal<T>.Default.Then().Operation()) {}
}

public class Update<TIn, TOut> : Modify<TIn, TOut> where TOut : class
{
	protected Update(IScopes scopes, IQuery<TIn, TOut> query) : this(scopes.Then().Use(query).Edit.Single()) {}

	protected Update(IScopes scopes, ISelecting<TIn, TOut> selecting)
		: this(new SelectForEdit<TIn, TOut>(scopes, selecting)) {}

	protected Update(IEdit<TIn, TOut> select) : base(@select, UpdateLocal<TOut>.Default.Then().Operation().Out()) {}
}