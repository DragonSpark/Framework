using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content;

sealed class UpdateMonitor : ICondition, ICommand
{
	readonly IMutable<bool> _active;
	readonly IMutable<bool> _state;

	public UpdateMonitor(IMutable<bool> state) : this(new Switch(), state) {}

	public UpdateMonitor(IMutable<bool> active, IMutable<bool> state)
	{
		_active = active;
		_state  = state;
	}

	public void Execute(None _)
	{
		_state.Down();
		_active.Up();
	}

	public bool Get(None _) => _active.Down();
}