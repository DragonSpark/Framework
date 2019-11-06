using System;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Commands
{
	sealed class SelectedAssignment<TIn, TOut> : IAssign<TIn, TOut>
	{
		readonly Func<TIn, ICommand<TOut>> _select;

		public SelectedAssignment(Func<TIn, ICommand<TOut>> select) => _select = select;

		public void Execute(Pair<TIn, TOut> parameter) => _select(parameter.Key).Execute(parameter.Value);
	}
}