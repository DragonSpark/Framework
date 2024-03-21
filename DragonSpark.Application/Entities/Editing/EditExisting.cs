using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

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
		var editor = new Editor(context, await disposable.Await());
		context.ChangeTracker.TrackGraph(parameter, x => x.Entry.State = EntityState.Unchanged);
		if (_reload)
		{
			await context.Entry(parameter).ReloadAsync().ConfigureAwait(false);
		}

		return new(editor, parameter);
	}
}