using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class EditExisting<T> : ISelecting<T, Edit<T>> where T : class
{
	readonly IScopes _scopes;

	protected EditExisting(IScopes scopes) => _scopes = scopes;

	public async ValueTask<Edit<T>> Get(T parameter)
	{
		var (context, disposable) = _scopes.Get();
		var editor = new Editor(context, await disposable.Await());
		editor.Attach(parameter);
		await context.Entry(parameter).ReloadAsync().ConfigureAwait(false);
		return new(editor, parameter);
	}
}