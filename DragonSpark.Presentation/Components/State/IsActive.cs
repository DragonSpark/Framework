﻿using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Properties;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Components.State;

public sealed class IsActive : ICondition<object>
{
	public static IsActive Default { get; } = new();

	IsActive() : this(ActiveState.Default) {}

	readonly ISelect<object, bool> _store;

	public IsActive(ISelect<object, bool> store) => _store = store;

	public bool Get(object parameter) => parameter is IActivityReceiver ar ? ar.Active : _store.Get(parameter);
}

//TODO

sealed class ActiveState : IProperty<object, bool>
{
	public static ActiveState Default { get; } = new();

	ActiveState() : this(Start.A.Selection<object>().By.Calling(_ => false).Stores().New()) {}

	readonly ITable<object, bool> _store;

	public ActiveState(ITable<object, bool> store) => _store = store;

	public ICondition<object> Condition => _store.Condition;

	public bool Get(object parameter) => _store.Get(parameter);

	public void Execute(Pair<object, bool> parameter)
	{
		_store.Execute(parameter);
	}
}
