using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Selection.Stores
{
	public class DelegatedTable<TIn, TOut> : ITable<TIn, TOut>
	{
		readonly Action<(TIn, TOut)> _assign;
		readonly Func<TIn, TOut>     _get;
		readonly Func<TIn, bool>     _remove;

		public DelegatedTable(ITable<TIn, TOut> source)
			: this(source.Condition, source.Assign, source.Get, source.Remove) {}

		// ReSharper disable once TooManyDependencies
		public DelegatedTable(Func<TIn, bool> contains, Action<(TIn, TOut)> assign,
		                      Func<TIn, TOut> get, Func<TIn, bool> remove)
			: this(new Condition<TIn>(contains), assign, get, remove) {}

		// ReSharper disable once TooManyDependencies
		public DelegatedTable(ICondition<TIn> contains, Action<(TIn, TOut)> assign,
		                      Func<TIn, TOut> get,
		                      Func<TIn, bool> remove)
		{
			Condition = contains;
			_assign   = assign;
			_get      = get;
			_remove   = remove;
		}

		public ICondition<TIn> Condition { get; }

		public TOut Get(TIn key) => _get(key);

		public bool Remove(TIn key) => _remove(key);

		public void Execute(Pair<TIn, TOut> parameter) => _assign(parameter.Key.Tuple(parameter.Value));
	}
}