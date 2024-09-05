using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public class UpdateActivityReceiverBase : IUpdateActivityReceiver
{
	readonly IUpdateActivity       _update;
	readonly ISelect<object, bool> _active;
	readonly bool                  _redraw;

	protected UpdateActivityReceiverBase(bool redraw) : this(UpdateActivity.Default, ActiveState.Default, redraw) {}

	protected UpdateActivityReceiverBase(IUpdateActivity update, ISelect<object, bool> active, bool redraw)
	{
		_update      = update;
		_active      = active;
		_redraw = redraw;
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
			             ? receiver.Complete(_redraw)
			             : ValueTask.CompletedTask;
		return result;
	}
}