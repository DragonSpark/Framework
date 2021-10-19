using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Save<T> : Update<T> where T : class
{
	public Save(IEnlistedScopes scopes) : base(scopes) {}
}

public class Save<TIn, TOut> : IOperation<TIn> where TOut : class
{
	readonly ISelecting<TIn, TOut> _select;
	readonly IOperation<TOut>      _save;

	public Save(ISelecting<TIn, TOut> select, Save<TOut> save) : this(@select, An.Operation(save)) {}

	public Save(ISelecting<TIn, TOut> select, IOperation<TOut> save)
	{
		_select = select;
		_save   = save;
	}

	public async ValueTask Get(TIn parameter)
	{
		var instance = await _select.Await(parameter);
		await _save.Await(instance);
	}
}