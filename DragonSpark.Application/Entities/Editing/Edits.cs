﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Edits<TIn, T> : IEdit<TIn, T>
{
	readonly IScopes            _scopes;
	readonly ISelecting<TIn, T> _select;

	public Edits(IScopes scopes, ISelecting<TIn, T> select)
	{
		_scopes = scopes;
		_select = select;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, disposable) = _scopes.Get();
		var instance = await _select.Await(parameter);
		return new(new Editor(context, await disposable.Await()), instance);
	}
}