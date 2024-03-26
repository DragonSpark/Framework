using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Edits<TIn, T> : IEdit<TIn, T>
{
	readonly IContexts            _contexts;
	readonly ISelecting<TIn, T> _select;

	public Edits(IContexts contexts, ISelecting<TIn, T> select)
	{
		_contexts = contexts;
		_select = select;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, disposable) = _contexts.Get();
		var instance = await _select.Await(parameter);
		return new(new Editor(context, disposable), instance);
	}
}