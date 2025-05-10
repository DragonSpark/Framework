using DragonSpark.Model.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

class Class1 {}

sealed class ActivityReceiverModel : IActivityReceiver
{
	readonly Stack<ActivityReceiverState> _stack;
	readonly Renderings                   _renderings;

	public ActivityReceiverModel(Renderings renderings) : this([], renderings) {}

	public ActivityReceiverModel(Stack<ActivityReceiverState> stack, Renderings renderings)
	{
		_stack      = stack;
		_renderings = renderings;
	}

	public ICommand<IRenderAware> Add => _renderings.Add;

	public ICommand<IRenderAware> Remove => _renderings.Remove;

	public ActivityOptions? Current => _stack.TryPeek(out var state) ? state.Options : null;

	public bool Active => _stack.Count > 0;

	public ValueTask Get(ActivityReceiverState parameter)
	{
		_stack.Push(parameter);
		return ValueTask.CompletedTask;
	}

	public ValueTask<ActivityReceiverState?> Get()
	{
		var last = _stack.TryPop(out var state) && _stack.Count == 0 ? state : default(ActivityReceiverState?);
		return new(last);
	}
}