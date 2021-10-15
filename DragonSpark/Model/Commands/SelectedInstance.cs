using System;

namespace DragonSpark.Model.Commands;

sealed class SelectedInstance<TIn, TOut> : IAssign<TIn, TOut>
{
	readonly Func<TIn, ICommand<TOut>> _select;

	public SelectedInstance(Func<TIn, ICommand<TOut>> select) => _select = select;

	public void Execute(Pair<TIn, TOut> parameter) => _select(parameter.Key).Execute(parameter.Value);
}