using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly IActiveContent<T> _previous;

	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous, previous.Monitor, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(IActiveContent<T> previous, IUpdateMonitor refresh, IResulting<T?> resulting)
		: base(resulting)
	{
		_previous = previous;
		Monitor   = refresh;
	}

	public IUpdateMonitor Monitor { get; }
}