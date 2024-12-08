using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

[UsedImplicitly]
public class CommitAwareEdits<T> : CommitAwareEdits<T, T>
{
	protected CommitAwareEdits(IScopes scopes) : base(scopes, Start.A.Selection<T>().By.Self.Operation().Out()) {}
}

public class CommitAwareEdits<TIn, T> : IEdit<TIn, T>
{
	readonly IScopes          _scopes;
	readonly ISelecting<TIn, T> _select;

	public CommitAwareEdits(IScopes scopes, ISelecting<TIn, T> select)
	{
		_scopes = scopes;
		_select   = select;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, disposable) = _scopes.Get();
		var instance = await _select.Await(parameter);
		var previous = new Editor(context, disposable);
		var editor   = new CommitAwareEditor(context.Database, previous);
		return new(editor, instance);
	}
}