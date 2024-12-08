using DragonSpark.Compose;
using System.Threading.Tasks;

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

	public async ValueTask<Edit<T>> Get(T parameter)
	{
		var (context, disposable) = _scopes.Get();
		var editor = new Editor(context, disposable);
		editor.Attach(parameter);
		if (_reload)
		{
			await context.Entry(parameter).ReloadAsync().Await();
		}

		return new(editor, parameter);
	}
}