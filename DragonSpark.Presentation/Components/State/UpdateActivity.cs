using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Properties;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

sealed class UpdateActivity : IUpdateActivity
{
	public static UpdateActivity Default { get; } = new();

	UpdateActivity() : this(Activities.Default.Get, ActiveState.Default) {}

	readonly Func<object, Stack<object>> _activities;
	readonly IProperty<object, bool>     _active;

	public UpdateActivity(Func<object, Stack<object>> activities, IProperty<object, bool> active)
	{
		_activities = activities;
		_active     = active;
	}

	public void Execute(Pair<object, object> parameter)
	{
		var (key, value) = parameter;
		_active.Assign(key, true);
		_activities(key).Push(value);
	}

	public void Execute(object parameter)
	{
		var store = _activities(parameter);
		if (store.TryPop(out _) && store.Count == 0)
		{
			_active.Assign(parameter, false);
		}
	}
}