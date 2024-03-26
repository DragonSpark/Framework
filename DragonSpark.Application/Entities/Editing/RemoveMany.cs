using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class RemoveMany<TIn, TOut> : IOperation<TIn> where TOut : class
{
	readonly IEditMany<TIn, TOut> _edit;

	protected RemoveMany(IEnlistedScopes scopes, IQuery<TIn, TOut> query)
		: this(new EditMany<TIn, TOut>(scopes, query)) {}

	public RemoveMany(IEditMany<TIn, TOut> edit) => _edit = edit;

	public async ValueTask Get(TIn parameter)
	{
		using var edit = await _edit.Await(parameter);
		foreach (var remove in edit.Subject)
		{
			edit.Remove(remove);
		}

		await edit.Await();
	}
}