using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditExisting<T> : IEdit<T> where T : class
{
	readonly IScopes _scopes;
	readonly bool    _reload;

	protected EditExisting(IScopes scopes, bool reload = true)
	{
		_scopes = scopes;
		_reload = reload;
	}

	[MustDisposeResource]
	public async ValueTask<Edit<T>> Get(T parameter)
	{
		var (context, disposable) = _scopes.Get();
		var editor = new Editor(context, disposable);
		editor.Attach(parameter);
		if (_reload)
		{
			await context.Entry(parameter).ReloadAsync().Off();
		}

		return new(editor, parameter);
	}
}
