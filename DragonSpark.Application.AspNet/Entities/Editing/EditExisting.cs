using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
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

	[MustDisposeResource]
	public async ValueTask<Edit<T>> Get(Stop<T> parameter)
	{
		var (context, disposable) = _scopes.Get();
		var editor = new Editor(context, disposable, parameter);
		editor.Attach(parameter.Subject);
		if (_reload)
		{
			await context.Entry(parameter.Subject).ReloadAsync(parameter).Off();
		}

		return new(editor, parameter);
	}
}
