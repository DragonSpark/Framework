using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class Edits<TIn, T> : IEdit<TIn, T>
{
	readonly IScopes            _scopes;
	readonly IStopAware<TIn, T> _select;

	public Edits(IScopes scopes, IStopAware<TIn, T> select)
	{
		_scopes = scopes;
		_select = select;
	}

	[MustDisposeResource]
	public async ValueTask<Edit<T>> Get(Stop<TIn> parameter)
	{
		var (context, disposable) = _scopes.Get();
		var instance = await _select.Off(parameter);
		return new(new Editor(context, disposable, parameter), instance);
	}
}
