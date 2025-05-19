using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class RemoveMany<TIn, TOut> : StopAdaptor<TIn> where TOut : class
{
	protected RemoveMany(IEnlistedScopes scopes, IQuery<TIn, TOut> query)
		: this(new EditMany<TIn, TOut>(scopes, query)) {}

	public RemoveMany(IEditMany<TIn, TOut> edit) : base(new RemoveManyWithStop<TIn, TOut>(edit)) {}
}

// TODO
public class RemoveManyWithStop<TIn, TOut> : IStopAware<TIn> where TOut : class
{
	readonly IEditMany<TIn, TOut> _edit;

	public RemoveManyWithStop(IEditMany<TIn, TOut> edit) => _edit = edit;

	public async ValueTask Get(Stop<TIn> parameter)
	{
		using var edit = await _edit.Off(parameter);
		foreach (var remove in edit.Subject)
		{
			edit.Remove(remove);
		}

		await edit.Off();
	}
}