using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Collections.Groups;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Commands;

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

	public ICommand<T> Get((IList<T>, ICommand<T>) parameter)
		=> parameter.Item2
		            .Then()
		            .Selection()
		            .Unless.Input.Is(_condition)
		            .ThenUse(new InsertItemCommand<T>(parameter.Item1, _index).Then().Selection())
		            .ToCommand();
}