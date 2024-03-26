using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class EditExisting<T> : IEdit<T> where T : class
{
	readonly IContexts _contexts;
	readonly bool    _reload;

	protected EditExisting(IContexts contexts, bool reload = true)
	{
		_contexts = contexts;
		_reload = reload;
	}

	public async ValueTask<Edit<T>> Get(T parameter)
	{
		var (context, disposable) = _contexts.Get();
		var editor = new Editor(context, disposable);
		editor.Attach(parameter);
		if (_reload)
		{
			await context.Entry(parameter).ReloadAsync().ConfigureAwait(false);
		}

		return new(editor, parameter);
	}
}