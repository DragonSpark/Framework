using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

public sealed class ClearActivities : ICommand<IActivityReceiver>
{
	public static ClearActivities Default { get; } = new();

	ClearActivities() : this(Activities.Default) {}

	readonly ISelect<IActivityReceiver, Stack<object>> _store;

	public ClearActivities(ISelect<IActivityReceiver, Stack<object>> store) => _store = store;

	public void Execute(IActivityReceiver parameter)
	{
		_store.Get(parameter).Clear();
	}
}