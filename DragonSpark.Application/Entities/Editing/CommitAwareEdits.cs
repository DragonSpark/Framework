using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

[UsedImplicitly]
public class CommitAwareEdits<T> : CommitAwareEdits<T, T>
{
	protected CommitAwareEdits(IContexts contexts) : base(contexts, Start.A.Selection<T>().By.Self.Operation().Out()) {}
}

public class CommitAwareEdits<TIn, T> : IEdit<TIn, T>
{
	readonly IContexts            _contexts;
	readonly ISelecting<TIn, T> _select;

	public CommitAwareEdits(IContexts contexts, ISelecting<TIn, T> select)
	{
		_contexts = contexts;
		_select = select;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var context  = _contexts.Get();
		var instance = await _select.Await(parameter);
		var previous = new Editor(context);
		var editor   = new CommitAwareEditor(context.Database, previous);
		return new(editor, instance);
	}
}