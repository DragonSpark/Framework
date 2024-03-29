﻿using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.State;

public sealed class ClearActivities : ICommand<object>
{
	public static ClearActivities Default { get; } = new();

	ClearActivities() : this(Activities.Default) {}

	readonly ISelect<object, ConcurrentBag<object>> _store;

	public ClearActivities(ISelect<object, ConcurrentBag<object>> store) => _store = store;

	public void Execute(object parameter)
	{
		_store.Get(parameter).Clear();
	}
}