using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly ICommand _previous;

	public ActivityAwareActiveContent(IActiveContent<T> previous, IActivityReceiver receiver)
		: this(previous, receiver, ActivityOptions.Redraw) {}

	public ActivityAwareActiveContent(IActiveContent<T> previous, IActivityReceiver receiver, ActivityOptions input)
		: this(previous, previous.Condition, new ActivityAwareResult<T>(previous, receiver, input)) {}

	public ActivityAwareActiveContent(ICommand previous, ICondition<None> refresh, IResulting<T?> resulting)
		: base(resulting)
	{
		_previous = previous;
		Condition = refresh;
	}

	public ICondition<None> Condition { get; }

	public void Execute(None parameter)
	{
		_previous.Execute(parameter);
	}
}