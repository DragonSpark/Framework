﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

sealed class CommitAwareEdits<TIn, T> : IEdit<TIn, T>
{
	readonly IScopes            _scopes;
	readonly ISelecting<TIn, T> _select;

	public CommitAwareEdits(IScopes scopes, ISelecting<TIn, T> select)
	{
		_scopes = scopes;
		_select = select;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, disposable) = _scopes.Get();
		var instance = await _select.Await(parameter);
		var previous = new Editor(context, await disposable.Await());
		var editor   = new CommitAwareEditor(context.Database, previous);
		return new(editor, instance);
	}
}