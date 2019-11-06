using System;
using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Collections.Groups;

namespace DragonSpark.Model.Sequences.Collections.Commands
{
	sealed class InsertItemCommands<T> : IDecoration<IList<T>, ICommand<T>>
	{
		public static InsertItemCommands<T> Default { get; } = new InsertItemCommands<T>();

		InsertItemCommands() : this(DeclaredGroupIndexes<T>.Default.Condition, DeclaredGroupIndexes<T>.Default.Get) {}

		readonly ICondition<T> _condition;

		readonly Func<T, int> _index;

		public InsertItemCommands(ICondition<T> condition, Func<T, int> index)
		{
			_condition = condition;
			_index     = index;
		}

		public ICommand<T> Get(Decoration<IList<T>, ICommand<T>> parameter)
			=> parameter.Result
			            .ToSelect()
			            .Unless(_condition, new InsertItemCommand<T>(parameter.Parameter, _index).ToSelect())
			            .ToCommand();
	}
}