using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content;

sealed class UpdateMonitor : ICondition, ICommand
{
	readonly IMutable<bool> _state;
	readonly IMutable<int>  _counts;

	public UpdateMonitor(IMutable<int> counts) : this(new Switch(), counts) {}

	public UpdateMonitor(IMutable<bool> state, IMutable<int> counts)
	{
		_state  = state;
		_counts = counts;
	}

	public void Execute(None _)
	{
		_counts.Execute(0);
		_state.Execute(true);
	}

	public bool Get(None _) => _state.Down();
}