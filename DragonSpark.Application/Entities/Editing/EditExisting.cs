using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System;
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
		editor.Attach(parameter);
		if (_reload)
		{
			await context.Entry(parameter).ReloadAsync().ConfigureAwait(false);
		}

		return new(editor, parameter);
	}
}

// TODO

public class ModifyExisting<T> : IOperation<T>
{
	readonly IEdit<T>            _edit;
	readonly IOperation<Edit<T>> _modify;

	protected ModifyExisting(IEdit<T> edit, ICommand<Edit<T>> modify) : this(edit, modify.Then().Operation().Out()) {}

	protected ModifyExisting(IEdit<T> edit, ICommand<T> modify)
		: this(edit, Start.A.Selection<Edit<T>>().By.Calling(x => x.Subject).Terminate(modify).Operation().Out()) {}

	protected ModifyExisting(IEdit<T> edit, Action<T> modify) : this(edit, Start.A.Command(modify).Get()) {}

	protected ModifyExisting(IEdit<T> edit, IOperation<T> modify)
		: this(edit, Start.A.Selection<Edit<T>>().By.Calling(x => x.Subject).Select(modify).Out()) {}

	protected ModifyExisting(IEdit<T> edit, IOperation<Edit<T>> modify)
	{
		_edit   = edit;
		_modify = modify;
	}

	public async ValueTask Get(T parameter)
	{
		using var edit = await _edit.Await(parameter);
		await _modify.Await(edit);
		await edit.Await();
	}
}