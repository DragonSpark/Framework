using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores;

sealed class TableValueAdapter<TIn, TOut> : ISelect<TIn, TOut> where TIn : notnull
{
	readonly Func<TIn, TOut>                _default;
	readonly IReadOnlyDictionary<TIn, TOut> _store;

	public TableValueAdapter(IReadOnlyDictionary<TIn, TOut> store, Func<TIn, TOut> @default)
	{
		_store   = store;
		_default = @default;
	}

	public TOut Get(TIn parameter) => _store.TryGetValue(parameter, out var result) ? result : _default(parameter);
}