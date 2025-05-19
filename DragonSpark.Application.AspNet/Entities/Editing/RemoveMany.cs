using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class RemoveMany<TIn, TOut> : StopAdaptor<TIn> where TOut : class
{
	protected RemoveMany(IEnlistedScopes scopes, IQuery<TIn, TOut> query)
		: this(new EditMany<TIn, TOut>(scopes, query)) {}

	public RemoveMany(IEditMany<TIn, TOut> edit) : base(new RemoveManyDispatch<TIn, TOut>(edit)) {}
}