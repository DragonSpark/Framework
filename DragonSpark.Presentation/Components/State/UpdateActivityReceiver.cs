using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class UpdateActivityReceiver : IUpdateActivityReceiver
{
	public static UpdateActivityReceiver Default { get; } = new();

	UpdateActivityReceiver() : this(UpdateActivity.Default) {}

	readonly IUpdateActivity       _update;
	readonly ISelect<object, bool> _active;

	public UpdateActivityReceiver(IUpdateActivity update) : this(update, ActiveState.Default) {}

	public UpdateActivityReceiver(IUpdateActivity update, ISelect<object, bool> active)
	{
		_update = update;
		_active   = active;
	}

	public ValueTask Get(Pair<object, ActivityReceiver> parameter)
	{
		var (key, (instance, input)) = parameter;

		var start = !_active.Get(key);

		_update.Execute((key, instance));

		var result = start && key is IActivityReceiver receiver ? receiver.Start(input) : ValueTask.CompletedTask;
		return result;
	}

	public ValueTask Get(object parameter)
	{
		var prior = _active.Get(parameter);

		_update.Execute(parameter);

		var completed = prior && !_active.Get(parameter);
		var result = completed && parameter is IActivityReceiver receiver
			             ? receiver.Complete()
			             : ValueTask.CompletedTask;
		return result;
	}
}

// TODO

public sealed class UpdateActivityReceiverAlternate : IUpdateActivityReceiver
{
	public static UpdateActivityReceiverAlternate Default { get; } = new();

	UpdateActivityReceiverAlternate() : this(UpdateActivity.Default) {}

	readonly IUpdateActivity       _update;
	readonly ISelect<object, bool> _active;

	public UpdateActivityReceiverAlternate(IUpdateActivity update) : this(update, ActiveState.Default) {}

	public UpdateActivityReceiverAlternate(IUpdateActivity update, ISelect<object, bool> active)
	{
		_update = update;
		_active = active;
	}

	public ValueTask Get(Pair<object, ActivityReceiver> parameter)
	{
		var (key, (instance, input)) = parameter;

		var start = !_active.Get(key);

		_update.Execute((key, instance));

		var result = start && key is IActivityReceiver receiver ? receiver.Start(input) : ValueTask.CompletedTask;
		return result;
	}

	public ValueTask Get(object parameter)
	{
		var prior = _active.Get(parameter);

		_update.Execute(parameter);

		var completed = prior && !_active.Get(parameter);
		var result = completed && parameter is IActivityReceiver receiver
			             ? receiver.Complete(true)
			             : ValueTask.CompletedTask;
		return result;
	}
}